using Sugar.Utils.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace iml6yu.Wechat.Mp.Message
{
    /// <summary>
    /// 
    /// </summary>
    public class WechatMenus
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="appscret"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public static async Task<string> SendMenusAsync(string appid, string appscret, string json)
        {
            var token = await HttpHelper.GetJsonAsync<AccessTokenInfo>("https://api.weixin.qq.com/cgi-bin/token", new { grant_type = "client_credential", appid = appid, secret = appscret });
            var sendresult = SendData($"https://api.weixin.qq.com/cgi-bin/menu/create?access_token={token.AccessToken}", json);
            return sendresult;
        }

        /// <summary>
        /// 获取自定义菜单
        /// </summary>
        /// <param name="posturl">自定义菜单请求的地址</param>
        /// <param name="postData">自定义菜单内容</param>
        /// <returns></returns>
        private static string SendData(string posturl, string postData)
        {
            Stream outstream = null;
            Stream instream = null;
            StreamReader sr = null;
            HttpWebResponse response = null;
            HttpWebRequest request = null;
            Encoding encoding = Encoding.UTF8;
            byte[] data = null;
            if (postData.Length > 0) //有值代表创建菜单
            {
                data = encoding.GetBytes(postData);
            }

            // 准备请求...
            try
            {
                // 设置参数
                request = WebRequest.Create(posturl) as HttpWebRequest;
                CookieContainer cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                if (postData.Length > 0)
                {
                    request.Method = "POST"; //创建菜单
                }
                else
                {
                    request.Method = "GET"; //删除菜单
                }

                request.ContentType = "application/x-www-form-urlencoded";

                if (postData.Length > 0) //有值代表创建菜单
                {
                    request.ContentLength = data.Length;
                    outstream = request.GetRequestStream();
                    outstream.Write(data, 0, data.Length);
                    outstream.Close();
                }

                //发送请求并获取相应回应数据
                response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                instream = response.GetResponseStream();
                sr = new StreamReader(instream, encoding);
                //返回结果网页（html）代码
                string content = sr.ReadToEnd();
                string err = string.Empty;
                return content;
            }
            catch (Exception ex)
            {
                string err = ex.Message; 
                return err;
            }
        }
    }
}
