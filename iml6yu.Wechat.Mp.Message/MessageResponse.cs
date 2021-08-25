using System;
using System.Collections.Generic;
using System.Text;

namespace iml6yu.Wechat.Mp.Message
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class MessageResponse
    {
        /// <summary>
        /// 回复
        /// </summary>
        /// <param name="message"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public abstract string Response(MessageModel message, string content = null);
    }

    /// <summary>
    /// 文本消息回复类
    /// </summary>
    public class MessageResponseText : MessageResponse
    {/// <summary>
     /// 回复
     /// </summary>
     /// <param name="message"></param>
     /// <param name="content"></param>
     /// <returns></returns>
        public override string Response(MessageModel message, string content = null)
        {
            return $@"<xml>
<ToUserName><![CDATA[{message.FromUserName}]]></ToUserName>
<FromUserName><![CDATA[{message.ToUserName}]]></FromUserName>
<CreateTime>{(DateTime.Now - new DateTime(1970, 1, 1, 8, 0, 0)).TotalSeconds}</CreateTime>
<MsgType><![CDATA[text]]></MsgType>
<Content><![CDATA[{content ?? "你好，暂时无法理解你的意思，我还在学习中..."}]]></Content> 
<MsgId>{message.MsgId}</MsgId>
</xml>";
        }
    }

    /// <summary>
    /// 订阅消息回复类
    /// </summary>
    public class MessageResponseSubscribe : MessageResponse
    {
        /// <summary>
        /// 回复
        /// </summary>
        /// <param name="message"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public override string Response(MessageModel message, string content = null)
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

    /// <summary>
    /// 取消订阅回复类
    /// </summary>
    public class MessageResponseUnsubscribe : MessageResponse
    {
        /// <summary>
        /// 回复
        /// </summary>
        /// <param name="message"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public override string Response(MessageModel message, string content = null)
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
