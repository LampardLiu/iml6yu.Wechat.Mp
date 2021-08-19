using iml6yu.Wechat.Mp.Message.MessageModels;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace iml6yu.Wechat.Mp.Message
{
    public class BasicMessage
    {
        public static async Task<MessageResponse> ReceiveMessageAsync(Stream stream)
        {

            var body = await GetBodyAsync<MessageModel>(stream);
            switch ((BasicMessageType)Enum.Parse(typeof(BasicMessageType), body.MsgType.ToUpper()))
            {
                case BasicMessageType.TEXT:
                    return new MessageResponseText(body as TextMessageModel);
                case BasicMessageType.EVENT:
                    var eventBody = body as EventMessageModel;
                    var eventType = (BasicMessageEventType)Enum.Parse(typeof(BasicMessageEventType), eventBody.Event);
                    if (eventType == BasicMessageEventType.SUBSCRIBE)
                        return new MessageResponseSubscribe(eventBody); 
                    return new MessageResponseUnsubscribe(eventBody);
                default:
                    return new MessageResponseText(body as TextMessageModel);
            }
        }

        /// <summary>
        /// 获取body
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static async Task<T> GetBodyAsync<T>(Stream stream) where T : MessageModel
        {
            var content = await ReadStream2StringAsync(stream);
            var body = DeserializeXML<T>(content);
            return body;
        }

        private static async Task<string> ReadStream2StringAsync(Stream stream)
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

        private static T DeserializeXML<T>(string xml) where T : class
        {
            using (var reader = new StringReader(xml))
            {
                var serializer = new XmlSerializer(typeof(T), new XmlRootAttribute("xml"));
                var result = serializer.Deserialize(reader);
                return result as T;
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
