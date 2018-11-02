#include<stdio.h>
#include <sys/socket.h>   //connect,send,recv,setsockoptµÈ
#include <sys/types.h>      

#include <netinet/in.h>     // sockaddr_in, "man 7 ip" ,htons
#include <poll.h>             //poll,pollfd
#include <arpa/inet.h>   //inet_addr,inet_aton
#include <unistd.h>        //read,write
#include <netdb.h>         //gethostbyname

#include <error.h>         //perror
#include <stdio.h>
#include <errno.h>         //errno

#include <string.h>          // memset
#include<stdlib.h>
#ifdef HAVE_NETINET_IN_H
#include <netinet/in.h>
#endif

#include "sqlite3.h"
#include <pthread.h>
#include "list.h"

#define false 0
#define true 1

#define DATABASE_PATH  "./database/"
#define DATABASE_NAME  "server.db"
#define TABLE_ACCOUNT_NAME "account_info"

FILE *gLogFile = NULL;

struct thread{
    char name[32];
    pthread_t tid;
};

struct client{
    char ip[16];
    int port;
    struct thread thread;
    int connection_fd;

    sqlite3 *db;
    int account_state; //0:logout 1:login
    char connected_account[64];
};

struct online_account_info{
    char account[64];
    int port;
    struct client *client;
};


struct offline_msg_info {
   char from[64];
   char to[64];
   CCLinkList msgList;
};

struct offline_msg {
    char server_time[32];
    char msg[4096];
};

static CCLinkList g_online_account_list;  /*struct online_account_info*/
static CCLinkList g_offline_msg_list;  /*struct offline_msg_info*/


enum {
    CMD_NULL = 0,
    CMD_ACCOUNT_REGISTER,
    CMD_ACCOUNT_LOGIN,
    CMD_GET_FRIENDS,
    CMD_ADD_FRIENDS,
    CMD_DEL_FRIENDS,
    CMD_SEND_MSG,
    CMD_ACCOUNT_LOGOUT,
    CMD_FIND_ACCOUNTS,
    CMD_CHECK_ACCOUNT,
    CMD_GET_ACCOUNT_DETAIL_INFO,
    CMD_MAX,
};

enum {
    REPLY_OK = 0,
    REPLY_ERROR,
    REPLY_ERROR_ACCOUNT,
    REPLY_ERROR_PASSWORD,
    REPLY_ERROR_OFFLINE,

    REPLY_HAVE_MESSAGE = 0x1000,
};

char *getCmdString(int cmd) {
    switch(cmd) {
        case CMD_ACCOUNT_REGISTER:
            return "CMD_ACCOUNT_REGISTER";
        case CMD_ACCOUNT_LOGIN:
            return "CMD_ACCOUNT_LOGIN";
        case CMD_GET_FRIENDS:
            return "CMD_GET_FRIENDS";
        case CMD_ADD_FRIENDS:
            return "CMD_ADD_FRIENDS";
        case CMD_DEL_FRIENDS:
            return "CMD_DEL_FRIENDS";
        case CMD_SEND_MSG:
            return "CMD_SEND_MSG";
        case CMD_ACCOUNT_LOGOUT:
            return "CMD_ACCOUNT_LOGOUT";
        case CMD_FIND_ACCOUNTS:
            return "CMD_FIND_ACCOUNTS";
        case CMD_CHECK_ACCOUNT:
            return "CMD_CHECK_ACCOUNT";
        case CMD_GET_ACCOUNT_DETAIL_INFO:
            return "CMD_GET_ACCOUNT_DETAIL_INFO";
        default:
            break;
    }

    return "CMD_UNKNOWN";
}

int accessAccount(sqlite3* db, const char* account, const char* password, char verify_pwd)
{
    char match = 0;
    sqlite3_stmt *stmt = NULL;
    int ret = 0;
    char sql[512];

    if(!db || !account || (!password && verify_pwd)) {
        fprintf(gLogFile, "error info\n");
        return -1;
    }

    snprintf(sql, sizeof(sql), "select * from %s;", TABLE_ACCOUNT_NAME);
    fprintf(gLogFile, "exec sql \"%s\"\n", sql);
    ret = sqlite3_prepare_v2(db, sql, -1, &stmt, NULL);
    if(ret != SQLITE_OK) {
        fprintf(gLogFile, "prepare %s failed, ret = %d, sql len = %ld\n", sql, ret, sizeof(sql));
        return -1;
    }

    do {
        ret = sqlite3_step(stmt);
        if(ret == SQLITE_ROW) {
            const char *db_account = (const char *)sqlite3_column_text(stmt, 0);
            const char *db_password = (const char *)sqlite3_column_text(stmt, 1);
            if(strcmp(account, db_account) == 0) {
                match = 1;
                if(verify_pwd) {
                    if(strcmp(password, db_password) == 0) {
                        match = 2;
                        break;
                    }
                } else {
                        match = 2;
                        break;
                }
            }
        }
    } while(ret == SQLITE_ROW);

    sqlite3_finalize(stmt);

    if(match == 2) return 0;
    if(match == 1) return 1;

    return -1;
}

int getAccountDetailInfo(sqlite3* db, const char* account, char *out, int out_size)
{
    sqlite3_stmt *stmt;
    char sql[512];
    int ret = 0;

    int len = snprintf(sql, sizeof(sql), "select * from %s where account=\"%s\";", TABLE_ACCOUNT_NAME, account);

    fprintf(gLogFile, "exec sql \"%s\"\n", sql);
    ret = sqlite3_prepare_v2(db, sql, -1, &stmt, NULL);
    if(ret != SQLITE_OK) {
        fprintf(gLogFile, "prepare %s failed, ret = %d, sql len = %ld\n", sql, ret, sizeof(sql));
        return -1;
    }

    len = 0;
    do {
        ret = sqlite3_step(stmt);
        if(ret == SQLITE_ROW) {
            const char *db_account = (const char *)sqlite3_column_text(stmt, 0);
            const char *db_username = (const char *)sqlite3_column_text(stmt, 2);
            const char *db_sex = (const char *)sqlite3_column_text(stmt, 3);
            const char *db_state = (const char *)sqlite3_column_text(stmt, 4);
            len += snprintf(out+len, out_size-len, "%s;%s;%s;%s;", db_account, db_username, db_sex, db_state);
        }
    } while(ret == SQLITE_ROW);

    sqlite3_finalize(stmt);

    return 0;
}

int registerAccount(sqlite3* db, const char* account, char* register_data, char *state)
{
    int ret = accessAccount(db, account, NULL, false);
    if(ret >= 0) return 1;

    char *p = NULL, *password = NULL, *username = NULL, *sex = NULL;

    password = register_data;
    p = strchr(register_data, ';');
    if(p) *p = '\0';

    if(p) {
        username = p+1;
        p = strchr(p+1, ';');
        if(p) *p = '\0';
    }

    if(p) {
        sex = p+1;
        p = strchr(p+1, ';');
        if(p) *p = '\0';
    }

    char sql[512];

    snprintf(sql, sizeof(sql), "insert into %s values(\"%s\", \"%s\", \"%s\", \"%s\", \"%s\");", TABLE_ACCOUNT_NAME,
        account, password, username, sex, state);

    fprintf(gLogFile, "exec sql \"%s\"\n", sql);
    ret = sqlite3_exec(db, sql, NULL, NULL, NULL);
    if(ret != SQLITE_OK) {
        fprintf(gLogFile, "prepare %s failed, ret = %d, sql len = %ld\n", sql, ret, sizeof(sql));
        return -1;
    }
    snprintf(sql, sizeof(sql), "create table friend_%s (account varchar(64), name varchar(64));", account);
    fprintf(gLogFile, "exec sql \"%s\"\n", sql);
    ret = sqlite3_exec(db, sql, NULL, NULL, NULL);
    if(ret != SQLITE_OK) {
        fprintf(gLogFile, "prepare %s failed, ret = %d, sql len = %ld\n", sql, ret, sizeof(sql));
        return -1;
    }

    return 0;
}

int getFirends(sqlite3* db, const char* account, char* out, int out_size)
{
    sqlite3_stmt *stmt = NULL;
    int ret = 0, ret1 = 0, len = 0;
    char sql[512];

    if(!account || !out || out_size <= 0) {
        fprintf(gLogFile, "error info\n");
        return -1;
    }
    snprintf(sql, sizeof(sql), "select * from friend_%s;", account);
    fprintf(gLogFile, "exec sql \"%s\"\n", sql);
    ret = sqlite3_prepare_v2(db, sql, -1, &stmt, NULL);
    if(ret != SQLITE_OK) {
        fprintf(gLogFile, "prepare %s failed, ret = %d, sql len = %ld\n", sql, ret, sizeof(sql));
        return -1;
    }

    do {
        ret = sqlite3_step(stmt);
        if(ret == SQLITE_ROW) {
            const char *friend_account = (const char *)sqlite3_column_text(stmt, 0);
            const char *friend_name = (const char *)sqlite3_column_text(stmt, 1);
            //get state
            sqlite3_stmt *stmt1 = NULL;
            char* friend_state = NULL;
            snprintf(sql, sizeof(sql), "select * from %s where account=\"%s\";", TABLE_ACCOUNT_NAME, friend_account);
            fprintf(gLogFile, "exec sql \"%s\"\n", sql);
            ret1 = sqlite3_prepare_v2(db, sql, -1, &stmt1, NULL);
            if(ret1 == SQLITE_OK) {
                ret1 = sqlite3_step(stmt1);
                if(ret == SQLITE_ROW) {
                    friend_state = (char *)sqlite3_column_text(stmt1, 4);
                }
            }
            //
            len += snprintf(out+len, out_size-len, "%s;%s;%s;", friend_account, friend_name, (friend_state == NULL) ? "" : friend_state);

            if(stmt1) sqlite3_finalize(stmt1);
        }
    } while(ret == SQLITE_ROW);

    sqlite3_finalize(stmt);

    return 0;
}

int addFirends(sqlite3* db, const char* account, char* friend_info)
{
    sqlite3_stmt *stmt = NULL;
    int ret = 0, exist = 0;
    char sql[512];
    char *p = NULL, *friend_account = NULL, *friend_name = NULL;

    friend_account = friend_info;
    p = strchr(friend_info, ';');
    if(p) *p = '\0';

    if(p) {
        friend_name = p+1;
        p = strchr(p+1, ';');
        if(p) *p = '\0';
    }

    snprintf(sql, sizeof(sql), "select * from friend_%s;", account);
    fprintf(gLogFile, "exec sql \"%s\"\n", sql);
    ret = sqlite3_prepare_v2(db, sql, -1, &stmt, NULL);
    if(ret != SQLITE_OK) {
        fprintf(gLogFile, "prepare %s failed, ret = %d, sql len = %ld\n", sql, ret, sizeof(sql));
        return -1;
    }

    do {
        ret = sqlite3_step(stmt);
        if(ret == SQLITE_ROW) {
            const char *list_friend_account = (const char *)sqlite3_column_text(stmt, 0);
            if(strcmp(friend_account, list_friend_account) == 0) {
                exist = 1;
                break;
            }
        }
    } while(ret == SQLITE_ROW);

    sqlite3_finalize(stmt);

    if(exist) {
        fprintf(gLogFile, "account[%s] has exist\n", friend_account);
        return 1;
    }

    snprintf(sql, sizeof(sql), "insert into friend_%s values(\"%s\", \"%s\");", account, friend_account, friend_name);
    fprintf(gLogFile, "exec sql \"%s\"\n", sql);
    ret = sqlite3_exec(db, sql, NULL, NULL, NULL);
    if(ret != SQLITE_OK) {
        fprintf(gLogFile, "sqlite3_exec %s failed, ret = %d, sql len = %ld\n", sql, ret, sizeof(sql));
        return -1;
    }

    return 0;
}

int delFirends(sqlite3* db, const char* account, char* friend_info)
{
    return 0;
}

int find_accounts(sqlite3* db,const char* find_account, char* out, int out_size)
{
    sqlite3_stmt *stmt;
    char sql[512];
    int ret = 0;

    int len = snprintf(sql, sizeof(sql), "select * from %s", TABLE_ACCOUNT_NAME);
    if(find_account != NULL) {
        len += snprintf(sql + len, sizeof(sql) - len, " where account=\"%s\"", find_account);
    }
    len += snprintf(sql + len, sizeof(sql) - len, " order by state desc;");

    fprintf(gLogFile, "exec sql \"%s\"\n", sql);
    ret = sqlite3_prepare_v2(db, sql, -1, &stmt, NULL);
    if(ret != SQLITE_OK) {
        fprintf(gLogFile, "prepare %s failed, ret = %d, sql len = %ld\n", sql, ret, sizeof(sql));
        return -1;
    }

    len = 0;
    do {
        ret = sqlite3_step(stmt);
        if(ret == SQLITE_ROW) {
            const char *db_account = (const char *)sqlite3_column_text(stmt, 0);
            const char *db_username = (const char *)sqlite3_column_text(stmt, 2);
            const char *db_sex = (const char *)sqlite3_column_text(stmt, 3);
            const char *db_state = (const char *)sqlite3_column_text(stmt, 4);
            len += snprintf(out+len, out_size-len, "%s;%s;%s;%s;", db_account, db_username, db_sex, db_state);
        }
    } while(ret == SQLITE_ROW);

    sqlite3_finalize(stmt);

    return 0;
}

int sendMessage(const char* from_account, const char* to_account, int to_port, char* send_data, char *server_time)
{
    if(to_port <= 0) {
        fprintf(gLogFile, "custom account[%s] is offline\n", to_account);
        return 1;
    }

    char sendbuf[4096];
    snprintf(sendbuf, sizeof(sendbuf), "%d;%d;%s;%s;%s", REPLY_HAVE_MESSAGE, CMD_SEND_MSG, from_account, server_time, send_data);

    int len = write(to_port, sendbuf, strlen(sendbuf)+1);
    fprintf(gLogFile, "send data:%s, len=%d\n", sendbuf, len);

    return 0;
}

int storeMessage(const char* from_account, const char* to_account, char* send_data, char *server_time)
{
    struct offline_msg_info *msg_info = NULL;
    while(NULL != (msg_info = getListNextItem(&g_offline_msg_list, msg_info)))
    {
        if(strncmp(msg_info->from, from_account, sizeof(msg_info->from)) == 0 &&
           strncmp(msg_info->to, to_account, sizeof(msg_info->to)) == 0) {
            struct offline_msg *msg = malloc(sizeof(struct offline_msg));
            memset(msg, 0, sizeof(struct offline_msg));
            strncpy(msg->server_time, server_time, sizeof(msg->server_time));
            strncpy(msg->msg, send_data, sizeof(msg->msg));
            appendList(&msg_info->msgList, msg);
            break;
        }
    }
    if(msg_info == NULL) {
        struct offline_msg_info *msg_info = malloc(sizeof(struct offline_msg_info));
        memset(msg_info, 0, sizeof(struct offline_msg_info));
        strncpy(msg_info->from, from_account, sizeof(msg_info->from));
        strncpy(msg_info->to, to_account, sizeof(msg_info->to));
        initList(&msg_info->msgList);
        appendList(&g_offline_msg_list, msg_info);

        struct offline_msg *msg = malloc(sizeof(struct offline_msg));
        memset(msg, 0, sizeof(struct offline_msg));
        strncpy(msg->server_time, server_time, sizeof(msg->server_time));
        strncpy(msg->msg, send_data, sizeof(msg->msg));
        appendList(&msg_info->msgList, msg);
    }

    return 0;
}

int receiveOfflineMessage(const char *receive_account, int self_port)
{
    struct offline_msg_info *msg_info = NULL;
    while(NULL != (msg_info = getListNextItem(&g_offline_msg_list, msg_info)))
    {
        if(strncmp(msg_info->to, receive_account, sizeof(msg_info->to)) == 0) {
            struct offline_msg *msg = NULL;
            while(NULL != (msg = getListNextItem(&msg_info->msgList, msg))) {
                sendMessage(msg_info->from, msg_info->to, self_port, msg->msg, msg->server_time);
                removeList(&msg_info->msgList, msg);
                free(msg);
                msg = NULL;
            }
            deinitList(&msg_info->msgList);
            removeList(&g_offline_msg_list, msg_info);
            free(msg_info);
            msg_info = NULL;
            break;
        }
    }
}

int updateAccountState(sqlite3* db, const char* account, const char* state)
{
    char sql[512];
    int ret = 0;

    snprintf(sql, sizeof(sql), "update %s set state=\"%s\" where account=\"%s\";", TABLE_ACCOUNT_NAME, state, account);

    fprintf(gLogFile, "exec sql \"%s\"\n", sql);
    ret = sqlite3_exec(db, sql, NULL, NULL, NULL);
    if(ret != SQLITE_OK) {
        fprintf(gLogFile, "prepare %s failed, ret = %d, sql len = %ld\n", sql, ret, sizeof(sql));
        return -1;
    }

    return 0;
}


int procClientMessage(struct client* client, char* msg, int size, int *outcmd, char* out, int out_size)
{
    int cmd = 0, i = 0, ret = 0;
    char *account = NULL, *data = NULL, *p = NULL, *cmd_str = NULL;
    int reply_code = REPLY_OK;

    cmd_str = msg;
    p = strchr(msg, ';');
    if(p) *p = '\0';

    cmd = atoi(cmd_str);
    if(cmd <= CMD_NULL || cmd >= CMD_MAX) {
        fprintf(gLogFile, "receive error cmd = %d\n", cmd);
        return REPLY_ERROR;
    }

    if(p) {
        account = p+1;
        p = strchr(p+1, ';');
        if(p) *p = '\0';
    }

    if(p) {
        data = p+1;
    }
        
    fprintf(gLogFile, "%s receive data:cmd[%s],account[%s],data[%s]\n", client->thread.name, getCmdString(cmd), account, data);
    switch(cmd) {
        case CMD_ACCOUNT_REGISTER:
            ret = registerAccount(client->db, account, data, "offline");
            if(ret == 1) {
                fprintf(gLogFile, "account[%s] is already registered\n", account);
                reply_code = REPLY_ERROR_ACCOUNT;
                break;
            }
            if(ret < 0) {
                fprintf(gLogFile, "register account[%s] error\n", account);
                reply_code = REPLY_ERROR;
                break;
            }
            break;
        case CMD_ACCOUNT_LOGIN:
            ret = accessAccount(client->db, account, data, true);
            if(ret == 1) {
                fprintf(gLogFile, "password error for account[%s]\n", account);
                reply_code = REPLY_ERROR_PASSWORD;
            }
            if(ret == 0) {
                struct online_account_info *online_account = NULL;
                while(NULL != (online_account = getListNextItem(&g_online_account_list, online_account)))
                {
                    if(strncmp(online_account->account, account, sizeof(online_account->account)) == 0) {
                        online_account->port = client->connection_fd;
                        online_account->client->account_state = 0;
                        online_account->client = client;
                        break;
                    }
                }
                if(online_account == NULL) {
                    online_account = malloc(sizeof(struct online_account_info));
                    memset(online_account, 0, sizeof(struct online_account_info));
                    strncpy(online_account->account, account, sizeof(online_account->account));
                    online_account->port = client->connection_fd;
                    online_account->client = client;
                    appendList(&g_online_account_list, online_account);
                }
                fprintf(gLogFile, "account[%s] login success.\n", account);
                client->account_state = 1;
                strncpy(client->connected_account, account, sizeof(client->connected_account));

                updateAccountState(client->db, account, "online");

                receiveOfflineMessage(account, client->connection_fd);
            }
            if(ret < 0) {
                fprintf(gLogFile, "can not find account[%s], please register it.\n", account);
                reply_code = REPLY_ERROR_ACCOUNT;
            }
            break;
        case CMD_GET_FRIENDS:
            if(client->account_state <= 0) {
                reply_code = REPLY_ERROR_ACCOUNT;
                break;
            }
            ret = getFirends(client->db, client->connected_account, out, out_size);
            reply_code = (ret == 0) ? REPLY_OK : REPLY_ERROR;
            break;
        case CMD_ADD_FRIENDS:
            if(client->account_state <= 0) {
                reply_code = REPLY_ERROR_ACCOUNT;
                break;
            }
            ret = addFirends(client->db, client->connected_account, data);
            reply_code = (ret == 0 || ret == 1) ? REPLY_OK : REPLY_ERROR;
            break;
        case CMD_DEL_FRIENDS:
            if(client->account_state <= 0) {
                reply_code = REPLY_ERROR_ACCOUNT;
                break;
            }
            ret = delFirends(client->db, client->connected_account, data);
            reply_code = (ret == 0) ? REPLY_OK : REPLY_ERROR;
            break;
        case CMD_SEND_MSG:
            if(client->account_state <= 0) {
                reply_code = REPLY_ERROR_OFFLINE;
                break;
            }
            char *from, *to;
            int to_port = -1;
            from = client->connected_account;
            to = data;
            p = strchr(data, ';');
            if(p) *p = '\0';

            if(p) data = p+1;

            if(!from || !to || !data) {
                reply_code = REPLY_ERROR;
                break;
            }

            struct online_account_info *online_account = NULL;
            while(NULL != (online_account = getListNextItem(&g_online_account_list, online_account)))
            {
                if(strncmp(online_account->account, to, sizeof(online_account->account)) == 0) {
                    to_port = online_account->port;
                    break;
                }
            }

            fprintf(gLogFile, "send msg:from[%s] to[%s] port[%d]\n", from, to, to_port);

            char server_time[32];
            time_t t;
            struct tm * lt;
            time (&t);
            lt = localtime (&t);
            snprintf(server_time, sizeof(server_time), "%04d/%02d/%02d %02d:%02d:%02d", lt->tm_year+1900, lt->tm_mon+1, lt->tm_mday, lt->tm_hour, lt->tm_min, lt->tm_sec);

            if(to_port > 0) {
                ret = sendMessage(from, to, to_port, data, server_time);
            } else {
                ret = storeMessage(from, to, data, server_time);
            }
            reply_code = (ret == 0) ? REPLY_OK : REPLY_ERROR;
            break;
        case CMD_ACCOUNT_LOGOUT:
            if(client->account_state == 1) {
                ret = accessAccount(client->db, client->connected_account, NULL, false);
                if(ret != 0)  reply_code = REPLY_ERROR;

                struct online_account_info *online_account = NULL;
                while(NULL != (online_account = getListNextItem(&g_online_account_list, online_account)))
                {
                    if(strncmp(online_account->account, client->connected_account, sizeof(online_account->account)) == 0) {
                        removeList(&g_online_account_list, online_account);
                        free(online_account);
                        online_account = NULL;
                        break;
                    }
                }
                client->account_state = 0;
                updateAccountState(client->db, client->connected_account, "offline");

                fprintf(gLogFile, "account[%s] logout success\n", client->connected_account);
                client->connected_account[0] = '\0';
            } else {
                reply_code = REPLY_ERROR;
            }
            break;
        case CMD_FIND_ACCOUNTS:
        {
            char *find_account = (data == NULL || data[0] == '\0') ? NULL : data;
            ret = find_accounts(client->db, find_account, out, out_size);
            if(ret != 0) {
                reply_code = REPLY_ERROR;
                fprintf(gLogFile, "find account[%s] failed", data);
            }
        }
            break;
        case CMD_CHECK_ACCOUNT:
            if(account == NULL) {
                fprintf(gLogFile, "check null account error.\n");
                reply_code = REPLY_ERROR;
                break;
            }
            ret = accessAccount(client->db, account, NULL, false);
            if(ret >= 0) {
                fprintf(gLogFile, "account[%s] exist.\n", account);
                reply_code = REPLY_ERROR_ACCOUNT;
            }
            break;
        case CMD_GET_ACCOUNT_DETAIL_INFO:
        {
            char *find_account = (data == NULL || data[0] == '\0') ? NULL : data;
            if(find_account == NULL) {
                fprintf(gLogFile, "check null friend account error.\n");
                reply_code = REPLY_ERROR;
                break;
            }
            ret = accessAccount(client->db, find_account, NULL, false);
            if(ret < 0) {
                fprintf(gLogFile, "account[%s] not exist.\n", find_account);
                reply_code = REPLY_ERROR_ACCOUNT;
                break;
            }
            ret = getAccountDetailInfo(client->db, find_account, out, out_size);
            reply_code = (ret == 0) ? REPLY_OK : REPLY_ERROR;
        }
            break;
        default:
            break;
    }

    *outcmd = cmd;

    return reply_code;
}

void* clientThread( void *param )
{
    struct client *client = (struct client*)param;
    int connect_fd = client->connection_fd;
    int reply = REPLY_OK;
    char reply_string[4096];
    char exit = 0;
    int cmd = 0;

    fprintf(gLogFile, "%s start read %s:%d\n", client->thread.name, client->ip, client->port);

    char recvbuf[4096];
    char replybuf[4096];

    do {
        cmd = 0;
        memset(recvbuf,0,sizeof(recvbuf));
        memset(replybuf,0,sizeof(replybuf));
        int ret = read(connect_fd, recvbuf, sizeof(recvbuf));
        if(ret == 0) {
            fprintf(gLogFile, "connection broken\n");
            snprintf(recvbuf, sizeof(recvbuf), "%d;%s", CMD_ACCOUNT_LOGOUT, client->connected_account);
            exit = 1;
        }
        if(ret < 0) {
            fprintf(gLogFile, "error ret = %d\n", ret);
            continue;
        } else {
            reply = procClientMessage(client, recvbuf, sizeof(recvbuf), &cmd, replybuf, sizeof(replybuf));
        }

        //reply
        if(ret != 0) {
            snprintf(reply_string, sizeof(reply_string), "%d;%d;%s", reply, cmd, replybuf);
            write(connect_fd, reply_string, strlen(reply_string) + 1);
        }
    } while(client && !exit);

    close(connect_fd);
    fprintf(gLogFile, "%s exit\n", client->thread.name);

    return NULL;
}

int main(int argc, char* const argv[])
{
    if(argc > 1 && strcmp(argv[1], "") != 0) {
        gLogFile = fopen(argv[1], "a+");
    }

    if(gLogFile == NULL) {
        gLogFile = stdout;
    }

    int ret = 0;
    sqlite3 *db = NULL;
    char database_name[32];
    //open database
    snprintf(database_name, sizeof(database_name), "%s/%s", DATABASE_PATH, DATABASE_NAME);
    ret = sqlite3_open_v2(database_name, &db, SQLITE_OPEN_READWRITE, NULL);
    if(ret == SQLITE_CANTOPEN){
        ret = sqlite3_open_v2(database_name, &db, SQLITE_OPEN_READWRITE | SQLITE_OPEN_CREATE, NULL);
        if(ret != SQLITE_OK) {
            fprintf(gLogFile, "open database \"%s\" failed, ret = %d\n", database_name, ret);
            return 0;
        }
        sqlite3_stmt *stmt = NULL;
        char sql[512];
        snprintf(sql, sizeof(sql), "create table %s (account varchar(64), password varchar(64), username varchar(64), sex varchar(64), state varchar(64));", TABLE_ACCOUNT_NAME);
        fprintf(gLogFile, "exec sql \"%s\"\n", sql);
        ret = sqlite3_exec(db, sql, NULL, NULL, NULL);
        if(ret != SQLITE_OK) {
            fprintf(gLogFile, "prepare %s failed, ret = %d, sql len = %ld\n", sql, ret, sizeof(sql));
            return -1;
        }
    } else if(ret != SQLITE_OK){
        fprintf(gLogFile, "open database \"%s\" failed, ret = %d\n", database_name, ret);
        return 0;
    }
    //init list
    initList(&g_online_account_list);
    initList(&g_offline_msg_list);

    //start socket
    int listenfd;
    if((listenfd =socket(PF_INET,SOCK_STREAM,IPPROTO_TCP))<0)
    {
        fprintf(gLogFile, "ERROR\n");
    }
    struct sockaddr_in servaddr;
    memset(&servaddr,0,sizeof(servaddr));
    servaddr.sin_family=AF_INET;
    servaddr.sin_port = htons(5188);
    servaddr.sin_addr.s_addr = htonl(INADDR_ANY);
    int on = 1;
    if(setsockopt(listenfd,SOL_SOCKET,SO_REUSEADDR,&on,sizeof(on))< 0)
    {
        fprintf(gLogFile, "SETSOCKOPT\n");
    }
    if(bind(listenfd,(struct sockaddr*)&servaddr,sizeof(servaddr))< 0)
    {
        fprintf(gLogFile, "error\n");
    }
    if(listen(listenfd,SOMAXCONN)<0)
    {
        fprintf(gLogFile, "listen\n");
    }

    //listen clients
    struct sockaddr_in peeraddr;
    socklen_t peerlen = sizeof(peeraddr);
    int conn;
    unsigned long threadCnt = 0;
    while(1) {
        conn = accept(listenfd,(struct sockaddr*)&peeraddr,&peerlen);
        if(conn < 0)
        {
            fprintf(gLogFile, "accept failed\n");
            break;
        }
        fprintf(gLogFile, "ip= %s,port = %d connected\n",inet_ntoa(peeraddr.sin_addr),ntohs(peeraddr.sin_port));
        struct client *client = malloc(sizeof(struct client));
        memset(client, 0, sizeof(struct client));
        client->connection_fd = conn;
        client->db = db;
        strncpy(client->ip, inet_ntoa(peeraddr.sin_addr), sizeof(client->ip));
        client->port = ntohs(peeraddr.sin_port);
        snprintf(client->thread.name, sizeof(client->thread.name), "thread_%lu", threadCnt++);
        pthread_create(&client->thread.tid, NULL, clientThread, client);
        pthread_setname_np(&client->thread.tid, &client->thread.name);
        fflush(gLogFile);
    }

    //exit
    close(listenfd);
    fprintf(gLogFile, "server exit\n");
    fclose(gLogFile);
    return 0;
}
