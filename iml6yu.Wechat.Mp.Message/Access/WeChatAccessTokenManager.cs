using Sugar.Utils.Http;
using System;

namespace iml6yu.Wechat.Mp.Message
{
    /// <summary>
    /// accesstoken管理类
    /// </summary>
    public class WechatAccessTokenManager
    {
        private AccessTokenCacheManager cacheManager;
        /// <summary>
        /// token被刷新了
        /// </summary>
        public event Action<string, AccessTokenInfo> RefreshTokened;
        /// <summary>
        /// token超时回收触发
        /// </summary>
        public event Action<string> TokenEvictioned;
        /// <summary>
        /// 
        /// </summary>
        private WechatAccessTokenManager()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheManager"></param>
        private WechatAccessTokenManager(AccessTokenCacheManager cacheManager)
        {
            this.cacheManager = cacheManager;
            this.cacheManager.TokenEvictioned += CacheManager_TokenEvictioned;
        }

        /// <summary>
        /// 适用于只有一个appid的情况
        /// </summary>
        /// <param name="cacheManager"></param>
        /// <param name="option"></param>
        public WechatAccessTokenManager(AccessTokenCacheManager cacheManager, WechatAccessOption option) : this(cacheManager)
        {
            if (option != null)
                SetOption(option);
        }

        /// <summary>
        /// 获取accesstoken
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="appscret"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<AccessTokenInfo> GetAccessTokenAsync(string appid, string appscret)
        {
            var value = cacheManager.Get(appid);
            if (value != null) return value;
            return await RequestAccessTokenAsync(appid, appscret);
        }

        /// <summary>
        /// 获取accesstoken
        /// </summary>
        /// <param name="appid"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<AccessTokenInfo> GetAccessTokenAsync(string appid)
        {
            var value = cacheManager.Get(appid);
            if (value != null) return value;
            if (cacheManager.GetSecret(appid) == null)
                throw new WechatSecretNullException("当前appid第一次出现，请调用包含appsecret参数的方法");
            return await RequestAccessTokenAsync(appid, cacheManager.GetSecret(appid));
        }

        internal void SetOption(WechatAccessOption option)
        {
            cacheManager.Add(option.AppId, option.AppSecret);
        }

        public async System.Threading.Tasks.Task<AccessTokenInfo> RequestAccessTokenAsync(string appid, string appscret)
        {
            var accesstoken = await HttpHelper.GetJsonAsync<AccessTokenInfo>("https://api.weixin.qq.com/cgi-bin/token", new { grant_type = "client_credential", appid = appid, secret = appscret });
            if (accesstoken != null)
            {
                cacheManager.Add(appid, accesstoken);
                this.RefreshTokened?.Invoke(appid, accesstoken);
            }
            return accesstoken;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appid"></param>
        private void CacheManager_TokenEvictioned(string appid)
        {
            TokenEvictioned?.Invoke(appid);
            RequestAccessTokenAsync(appid, cacheManager.GetSecret(appid)).Wait(6000);
        }
    }
}
