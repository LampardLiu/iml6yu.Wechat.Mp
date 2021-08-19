using System;
using System.Collections.Generic;
using System.Text;

namespace iml6yu.Wechat.Mp.Message.MessageModels
{
    public class TextMessageModel : MessageModel
    {
        public string Content { get; set; }
        /// <summary>
        /// 消息id，64位整型
        /// </summary>
        public long MsgId { get; set; }
    }
}
