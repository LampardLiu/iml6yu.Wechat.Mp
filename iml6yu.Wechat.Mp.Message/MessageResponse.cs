using iml6yu.Wechat.Mp.Message.MessageModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace iml6yu.Wechat.Mp.Message
{
    public abstract class MessageResponse
    {
        public abstract string Response(string content = null);
    }

    public class MessageResponseText : MessageResponse
    {
        private TextMessageModel message;
        public MessageResponseText(TextMessageModel message)
        {
            this.message = message;
        }
        public override string Response(string content = null)
        {
            return $@"<xml>
                  <ToUserName><![CDATA[{message.FromUserName}]]></ToUserName>
                  <FromUserName><![CDATA[{message.ToUserName}]]></FromUserName>
                  <CreateTime>{(DateTime.Now - new DateTime(1970, 1, 1, 8, 0, 0)).TotalSeconds}</CreateTime>
                  <MsgType><![CDATA[text]]></MsgType>
                  <Content><![CDATA[{content}]]></Content> 
                  <MsgId>{message.MsgId}</MsgId>
                </xml>";
        }
    }

    public class MessageResponseSubscribe : MessageResponse
    {
        private EventMessageModel message;
        public MessageResponseSubscribe(EventMessageModel message)
        {
            this.message = (EventMessageModel)message;
        }
        public override string Response(string content = null)
        {
            return $@"<xml>
                  <ToUserName><![CDATA[{message.FromUserName}]]></ToUserName>
                  <FromUserName><![CDATA[{message.ToUserName}]]></FromUserName>
                  <CreateTime>{(DateTime.Now - new DateTime(1970, 1, 1, 8, 0, 0)).TotalSeconds}</CreateTime>
                  <MsgType><![CDATA[text]]></MsgType>
                  <Content><![CDATA[{content ?? "欢迎关注此公众号！\r\n如果遇到任何问题，请在这里告诉我！"}]]></Content> 
                  <MsgId>1</MsgId>
                </xml>";
        }
    }

    public class MessageResponseUnsubscribe : MessageResponse
    {
        private EventMessageModel message;
        public MessageResponseUnsubscribe(EventMessageModel message)
        {
            this.message = (EventMessageModel)message;
        }
        public override string Response(string content = null)
        {
            return $@"<xml>
                  <ToUserName><![CDATA[{message.FromUserName}]]></ToUserName>
                  <FromUserName><![CDATA[{message.ToUserName}]]></FromUserName>
                  <CreateTime>{(DateTime.Now - new DateTime(1970, 1, 1, 8, 0, 0)).TotalSeconds}</CreateTime>
                  <MsgType><![CDATA[text]]></MsgType>
                  <Content><![CDATA[{content ?? "欢迎关注此公众号！\r\n如果遇到任何问题，请在这里告诉我！"}]]></Content> 
                  <MsgId>1</MsgId>
                </xml>";
        }
    }
}
