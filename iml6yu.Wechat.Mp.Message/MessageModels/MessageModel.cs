using System;
using System.Collections.Generic;
using System.Text;

namespace iml6yu.Wechat.Mp.Message
{
    public class MessageModel
    {
        public string ToUserName { get; set; }
        public string FromUserName { get; set; }
        public int CreateTime { get; set; }
        public string MsgType { get; set; }
        public string Content { get; set; }
        /// <summary>
        /// 消息id，64位整型
        /// </summary>
        public long MsgId { get; set; }
        public string Event { get; set; }

    }
}
