using iml6yu.Wechat.Mp.Message.TemplateMessage;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace iml6yu.Wechat.Mp.Message.Test
{
    [TestClass]
    public class MessageTestCases
    {
        [TestMethod]
        public void TestMethod1()
        {
            var s = SerializeXML<Person>(new Person() { Age = 1, Name = "111" });
            Assert.IsTrue(false);
        }


        [TestMethod]
        public void TestDateTimeToInt()
        {
            //1629258132 
            var dt = new DateTime(1970, 1, 1, 8, 0, 0).AddSeconds(1629258132);
            Assert.IsTrue(false);
        }


        [TestMethod]
        public void TestStringToEnum()
        {
             
            
          var a= (BasicMessageType) Enum.Parse(typeof(BasicMessageType), "");
            Assert.AreEqual(a, BasicMessageType.TEXT);
        }

        [TestMethod]
        public async Task TestTemplateMessageAsync()
        {
            //TemplateMessageProvider p =
            ////new TemplateMessageProvider(
            /// new WeChatAccessOption("wx9b0f67e90ae6aff3", "68af606451a13737d0ae0bde2f31278b"),
            /// new WeChat.Access.WeChatAccessTokenManager(
            /// new AccessTokenCacheManager(new MemoryCache(new MemoryCacheOptions()), 
            /// new LoggerFactory())));
            var options = new WechatAccessOption("wx0aa96bb28e5c78d5", "e4c1caa02ea3b0b861f55a35397edff7")  ;
            BasicMessage message = new BasicMessage(options, new WechatAccessTokenManager(new AccessTokenCacheManager(new MemoryCache(new MemoryCacheOptions()), new LoggerFactory()), options));
            var result = await message.TemplateMessage.SendOffiAccountMessageAsync(new OffiAccountMessage()
            {
                TemplateId = "c0hrNkQ6skxcVx1Oq9P-ljO8QgAfW4_K36VwhV4T2aw",
                Url = "www.baidu.com",
                Data = new MessageContent()
                {
                    MessageTitle = new MessageContentItem("测试title"),
                    MessageDatas = new List<MessageContentItem>() { new MessageContentItem("数据1"), new MessageContentItem("数据2") },
                    Remark = new MessageContentItem("备注信息")
                }
            }, "oOIpd55JvM7LXJleWKmPTssByFuE");
            Assert.AreEqual(result.Count, 1);
            Assert.AreEqual(result.First().Value.ErrCode, 0);
        }
        private string SerializeXML<T>(T obj) where T : class
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
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
