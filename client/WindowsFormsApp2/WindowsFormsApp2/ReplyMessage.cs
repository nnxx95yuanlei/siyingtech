using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsFormsApp2
{

    public enum CMD
    {
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

    public enum REPLY
    {
        REPLY_OK = 0,
        REPLY_ERROR,
        REPLY_ERROR_ACCOUNT,
        REPLY_ERROR_PASSWORD,
        REPLY_ERROR_OFFLINE,

        REPLY_HAVE_MESSAGE = 0x1000,
    };

    public class ReplyMessage
    {
        private int myCmd;
        private int myReply;
        private string myMsg;
        public ReplyMessage(int cmd, int reply, string msg)
        {
            myCmd = cmd;
            myReply = reply;
            myMsg = msg;
        }

        public int getCmd()
        {
            return myCmd;
        }

        public int getReply()
        {
            return myReply;
        }

        public string getMsg()
        {
            return myMsg;
        }
    };
}
