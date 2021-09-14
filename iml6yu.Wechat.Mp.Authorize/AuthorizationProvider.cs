using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace iml6yu.Wechat.Mp.Authorization
{
    public class AuthorizationProvider
    {
        private string appid;
        private string appsecret;
        public AuthorizationProvider(string appid, string appsecret)
        {
            this.appid = appid;
            this.appsecret = appsecret;
        }

        /// <summary>
        /// 获取登录URL
        /// </summary>
        /// <param name="redirect"></param>
        /// <returns></returns>
        public string GetCodeUrl(string redirect)
        {
            return $"https://open.weixin.qq.com/connect/oauth2/authorize?appid={appid}&redirect_uri={HttpUtility.UrlEncode(redirect)}&response_type=code&scope=snsapi_base&state=iml6yu#wechat_redirect";
        }

        /// <summary>
        /// 获取OpenID
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public async Task<string> GetOpenIdAsync(string code, string state)
        {
            if ("iml6yu" != state) return $"error:state";
            return (await GetAccessTokenAsync(code))?.OpenId ?? "error:no openid";
        }

       
        private async Task<AccessTokenModel> GetAccessTokenAsync(string code)
        {
            var content = await (await new HttpClient().GetAsync($"https://api.weixin.qq.com/sns/oauth2/access_token?appid={appid}&secret={appsecret}&code={code}&grant_type=authorization_code")).Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<AccessTokenModel>(content);
            return result;
        }

        public async Task<AccessTokenModel> GetClientAccessTokenAsync(string appid,string secret)
        {
            var content = await (await new HttpClient().GetAsync($"https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={appid}&secret={secret}")).Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<AccessTokenModel>(content);
            return result;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public  WxUserInfo GetWxUserInfo(string access_token, string openid)
        { 
            var wxUserInfo_Url = string.Format("https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang=zh_CN", access_token, openid);
            var result = new  WxUserInfo();

            WebClient wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
            var get_result = wc.DownloadString(wxUserInfo_Url);
        
            if (!get_result.Contains("error"))
            {
                var wxApi_ResultModel = JsonConvert.DeserializeObject<WxUserInfo>(get_result);
                result = wxApi_ResultModel;
            }
            else
                throw new Exception(string.Format("wx获取用户信息接口错误，error：{0}", "get_result"));

            return result;
        }
    }
}
