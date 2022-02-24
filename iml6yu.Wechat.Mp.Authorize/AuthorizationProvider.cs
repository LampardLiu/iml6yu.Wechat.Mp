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

        /// <summary>
        /// 获取code认证的token
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<AccessTokenModel> GetAccessTokenModelAsync(string code, string state)
        {
            if ("iml6yu" != state) throw new ArgumentException($"参数state错误");
            return (await GetAccessTokenAsync(code));
        }

        private async Task<AccessTokenModel> GetAccessTokenAsync(string code)
        {
            var content = await (await new HttpClient().GetAsync($"https://api.weixin.qq.com/sns/oauth2/access_token?appid={appid}&secret={appsecret}&code={code}&grant_type=authorization_code")).Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<AccessTokenModel>(content);
            return result;
        }

        public async Task<AccessTokenModel> GetClientAccessTokenAsync(string appid, string secret)
        {
            var content = await (await new HttpClient().GetAsync($"https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={appid}&secret={secret}")).Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<AccessTokenModel>(content);
            return result;
        }

        /// <summary>
        /// 获取完整的用户信息
        /// </summary>
        /// <param name="client_access_token">客户端认证的token，是通过GetClientAccessTokenAsync方法返回的token</param>
        /// <param name="code_access_token">code认证的token，是通过GetAccessTokenModelAsync方法返回的token</param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public WxUserInfo GetWxUserWholeInfo(string client_access_token, string code_access_token, string openid)
        {
            var user1 = GetWxUserInfo(client_access_token, openid);
            var user2 = GetWxUserBaseInfo(code_access_token, openid);
            user1.country = user2?.country;
            user1.province = user2?.province;
            user1.city = user2?.city;
            user1.nickname = user2?.nickname;
            user1.headimgurl = user2?.headimgurl;
            user1.sex = user2?.sex;
            return user1;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public WxUserInfo GetWxUserInfo(string access_token, string openid)
        {
            var wxUserInfo_Url = string.Format("https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang=zh_CN", access_token, openid);
            var result = new WxUserInfo();

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
        /// <summary>
        /// 获取微信基础信息（包含nickname，地域，头像等）
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public WxUserInfo GetWxUserBaseInfo(string access_token, string openid)
        {
            var baseUserInfoUrl = string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN", access_token, openid);
            WxUserInfo result;
            WebClient wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
            var get_result = wc.DownloadString(baseUserInfoUrl);

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
