using iml6yu.Wechat.Mp.Message.TemplateMessage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace iml6yu.Wechat.Mp.Message
{ 
    public class BasicMessage
    {
        private Dictionary<BasicMessageType, Func<MessageModel, string>> configs;
        /// <summary>
        /// 发送模板消息的对象
        /// <code>
        /// var options = new WechatAccessOption("appid", "appsecret")  ;
        /// BasicMessage message = new BasicMessage(options, new WechatAccessTokenManager(new AccessTokenCacheManager(new MemoryCache(new MemoryCacheOptions()), new LoggerFactory()), options));
        /// var result = await message.TemplateMessage.SendOffiAccountMessageAsync(new OffiAccountMessage()
        /// {
        ///  TemplateId = "模板id",
        ///  Url = "详情地址",
        ///  Data = new MessageContent()
        ///      {
        ///         MessageTitle = new MessageContentItem("测试title"),
        ///         MessageDatas = new List《MessageContentItem》() { new MessageContentItem("数据1"), new MessageContentItem("数据2") },
        ///         Remark = new MessageContentItem("备注信息")
        ///         }
        ///   }, "目标用户OPENID");
        /// </code> 
        /// </summary>
        public TemplateMessageProvider TemplateMessage;
        public BasicMessage()
        {
            configs = new Dictionary<BasicMessageType, Func<MessageModel, string>>();
            this.ConfigAction<MessageResponseText>(BasicMessageType.TEXT).
                ConfigAction<MessageResponseSubscribe>(BasicMessageType.EVENT_SUBSCRIBE).
                ConfigAction<MessageResponseUnsubscribe>(BasicMessageType.EVENT_UNSUBSCRIBE);
        }

        public BasicMessage(WechatAccessOption option, WechatAccessTokenManager tokenManager) : this()
        {
            TemplateMessage = new TemplateMessageProvider(option, tokenManager);
        }
        /// <summary>
        /// 配置不同消息对应的行为
        /// </summary>
        /// <param name="messageType">消息类型</param>
        /// <param name="action">行为</param>
        /// <returns></returns>
        public BasicMessage ConfigAction(BasicMessageType messageType, Func<MessageModel, string> action)
        {
            if (!configs.ContainsKey(messageType))
                configs.Add(messageType, action);
            else
                configs[messageType] = action;
            return this;
        }

        /// <summary>
        /// 配置不同消息对应的处理行为
        /// </summary>
        /// <typeparam name="T">处理该消息的类，必须继承MessageResponse</typeparam>
        /// <param name="messageType">消息类型</param>
        /// <returns></returns>
        public BasicMessage ConfigAction<T>(BasicMessageType messageType) where T : MessageResponse
        {
            var instance = (MessageResponse)Activator.CreateInstance<T>();
            if (!configs.ContainsKey(messageType))
                configs.Add(messageType, new Func<MessageModel, string>(o =>
                 {
                     return instance.Response(o);
                 }));
            else
                configs[messageType] = new Func<MessageModel, string>(o =>
                {
                    return instance.Response(o);
                });
            return this;
        }

        /// <summary>
        /// 验证接入
        /// </summary>
        /// <param name="token"></param>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        public bool VerifyJoin(string token, string timestamp, string nonce, string signature)
        {
            List<string> list = new List<string>();
            list.Add(token);
            list.Add(timestamp);
            list.Add(nonce);
            list.Sort();
            string res = string.Join("", list.ToArray());
            Byte[] data1ToHash = Encoding.ASCII.GetBytes(res);
            byte[] hashvalue1 = ((HashAlgorithm)CryptoConfig.CreateFromName("SHA1")).ComputeHash(data1ToHash);

            StringBuilder sb = new StringBuilder();
            foreach (byte b in hashvalue1)
            {
                sb.Append(b.ToString("x2"));
            }
            return signature == sb.ToString();
        }

        public async Task<string> ReceiveMessageAsync(Stream stream)
        {
            var body = await GetBodyAsync<MessageModel>(stream);
            if (body.MsgType == "event")
            {
                switch (body.Event)
                {
                    case "subscribe":
                        if (configs.ContainsKey(BasicMessageType.EVENT_SUBSCRIBE))
                            return configs[BasicMessageType.EVENT_SUBSCRIBE].Invoke(body);
                        return string.Empty;
                    case "unsubscribe":
                        if (configs.ContainsKey(BasicMessageType.EVENT_UNSUBSCRIBE))
                            return configs[BasicMessageType.EVENT_UNSUBSCRIBE].Invoke(body);
                        return string.Empty;
                    default:
                        return string.Empty;
                }
            }
            else
            {
                var type = (BasicMessageType)Enum.Parse(typeof(BasicMessageType), body.MsgType.ToUpper());
                if (configs.ContainsKey(type))
                    return configs[type].Invoke(body);
                return string.Empty;
            }


            //
            //switch ()
            //{
            //    case BasicMessageType.TEXT:
            //        return new MessageResponseText(body);
            //    case BasicMessageType.EVENT: 
            //        var eventType = (BasicMessageEventType)Enum.Parse(typeof(BasicMessageEventType), body.Event.ToUpper());
            //        if (eventType == BasicMessageEventType.SUBSCRIBE)
            //            return new MessageResponseSubscribe(body);
            //        return new MessageResponseUnsubscribe(body);
            //    default:
            //        return new MessageResponseText(body);
            //}
        }

        /// <summary>
        /// 获取body
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="stream"></param>
        /// <returns></returns>
        private async Task<T> GetBodyAsync<T>(Stream stream) where T : MessageModel
        {
            var content = await ReadStream2StringAsync(stream);
            var body = DeserializeXML<T>(content);
            return body;
        }

        private async Task<string> ReadStream2StringAsync(Stream stream)
        {
            if (null == stream)
            {
                return string.Empty;
            }
            using (StreamReader reader = new StreamReader(stream))
            {
                return await reader.ReadToEndAsync();
            }
        }

        private T DeserializeXML<T>(string xml) where T : class
        {
            try
            {
                using (var reader = new StringReader(xml))
                {
                    var serializer = new XmlSerializer(typeof(T), new XmlRootAttribute("xml"));
                    var result = serializer.Deserialize(reader);
                    return result as T;
                }
            }
            catch (Exception ex)
            {
                ;
                throw;
            }

        }

        [Obsolete("反序列化后的结果不适用于微信")]
        private static string SerializeXML<T>(T obj) where T : class
        {
            using (MemoryStream Stream = new MemoryStream())
            {
                XmlSerializer xml = new XmlSerializer(typeof(T));
                //序列化对象  
                xml.Serialize(Stream, obj);
                Stream.Position = 0;
                using (StreamReader sr = new StreamReader(Stream))
                {
                    string str = sr.ReadToEnd();
                    return str;
                }
            }
        }
    }
}
