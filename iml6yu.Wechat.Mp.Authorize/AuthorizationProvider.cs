using Newtonsoft.Json;
using System;
using System.Net.Http;
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
    }
}
