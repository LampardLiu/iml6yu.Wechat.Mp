using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
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
