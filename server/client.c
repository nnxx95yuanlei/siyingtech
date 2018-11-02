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

#include <pthread.h>

#define ERR_EXIT(m)\
    do\
{\
    perror(m);\
    exit(EXIT_FAILURE);\
}while(0)

void* pollThread(void* param)
{
    int sock = *(int*)param;
    printf("poll from %d\n", sock);
    char recvbuf[4096]={0};
    char *p, *data;
    int reply = 0, cmd = 0;
    const char delim = ';';

    while(1) {
        reply = cmd = 0;
        int len = read(sock,recvbuf,sizeof(recvbuf));
        if(len == 0) break;
        if(len == 1 && recvbuf[0] == '\0') continue;
        p = strtok(recvbuf, &delim);
        reply = atoi(p);
        p = strtok(NULL, &delim);
        cmd = atoi(p);
        data = p + strlen(p) + 1;
        printf("reply[%d] for cmd[%d], data[%s]\n", reply, cmd, data);
    }

    printf("exit thread\n");
}

int main(void)
{
    int sock;
    if((sock =socket(PF_INET,SOCK_STREAM,IPPROTO_TCP))<0)
    {
        ERR_EXIT("ERROR");   
    }
    struct sockaddr_in servaddr;
    memset(&servaddr,0,sizeof(servaddr));
    servaddr.sin_family=AF_INET;
    servaddr.sin_port = htons(5188);
    servaddr.sin_addr.s_addr = inet_addr("127.0.0.1");//("144.34.132.4");
    if(connect(sock,(struct sockaddr*)&servaddr,sizeof(servaddr))< 0)
    {
    ERR_EXIT("connect");
    }

    pthread_t pid;
    pthread_create(&pid, NULL, pollThread, &sock);

    char sendbuf[1024]={0};
    while(fgets(sendbuf,sizeof(sendbuf),stdin)!=NULL)
    {
    if(sendbuf[strlen(sendbuf)-1] == '\n') sendbuf[strlen(sendbuf)-1] = '\0';
    if(strlen(sendbuf) == 0) continue;
    write(sock,sendbuf,sizeof(sendbuf));
    //fputs(recvbuf,stdout);
    memset(sendbuf,0,sizeof(sendbuf));
    //memset(recvbuf,0,sizeof(recvbuf));
    }
    close(sock);
    return 0;
}