using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace iml6yu.Wechat.Mp.Authorization
{
    /// <summary>
    /// 扩展类
    /// </summary>
    public static class WechatExtensions
    {
        #region 启动配置

        /// <summary>
        /// 配置 AuthorizationProvider，支持微信认证
        /// </summary>
        /// <param name="services"></param>
        /// <param name="appid"></param>
        /// <param name="appsecret"></param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection UseWechatAuthorization(this IServiceCollection services, string appid, string appsecret)
        {
            return services.AddSingleton(new AuthorizationProvider(appid, appsecret));
        }
        #endregion
    }
}
