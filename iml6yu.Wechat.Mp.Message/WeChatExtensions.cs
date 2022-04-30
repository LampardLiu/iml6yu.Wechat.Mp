using iml6yu.Wechat.Mp.Message.TemplateMessage;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace iml6yu.Wechat.Mp.Message
{
    /// <summary>
    /// 扩展类
    /// </summary>
    public static class WechatExtensions
    {
        #region 启动配置

        /// <summary>
        /// 添加消息处理机制，同时支持模板消息
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        /// <param name="acOption">wechat的配置信息</param>
        /// <param name="configMessageActions">基础消息行为配置</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection UseWechatMessage(this IServiceCollection services, Action<WechatAccessOption> acOption, Action<BasicMessage> configMessageActions)
        {
            WechatAccessOption option = GetOption(acOption);
            return AddBasicMessage(services, configMessageActions, option);
        }

        /// <summary>
        /// 添加消息处理机制，同时支持模板消息
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        /// <param name="acOption">wechat的配置信息</param>
        /// <param name="configMessageActions">基础消息行为配置</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection UseWechatMessage(this IServiceCollection services, IConfigurationSection section, Action<BasicMessage> configMessageActions)
        {
            WechatAccessOption option = GetSectionOption(section);
            return AddBasicMessage(services, configMessageActions, option);
        }

        /// <summary>
        /// 添加模板处理工具类
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        /// <param name="acOption">配置信息</param>
        /// <returns></returns>
        public static IServiceCollection UseWechatTemplateMessage(this IServiceCollection services, Action<WechatAccessOption> acOption, Action<string, AccessTokenInfo> refreshAction, Action<string> evictionedAction)
        {
            WechatAccessOption option = GetOption(acOption);
            return AddWechatTemplateMessage(services, option, refreshAction,evictionedAction);
        }


        /// <summary>
        /// 添加模板处理工具类
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        /// <param name="section">配置节点名称</param>
        /// <returns></returns>
        public static IServiceCollection UseWechatTemplateMessage(this IServiceCollection services, IConfigurationSection section, Action<string, AccessTokenInfo> refreshAction, Action<string> evictionedAction)
        {
            WechatAccessOption option = GetSectionOption(section);
            return AddWechatTemplateMessage(services, option, refreshAction, evictionedAction);
        }

        private static WechatAccessOption GetOption(Action<WechatAccessOption> acOption)
        {
            WechatAccessOption option = new WechatAccessOption();
            acOption?.Invoke(option);
            if (string.IsNullOrEmpty(option.AppId) || string.IsNullOrEmpty(option.AppSecret))
                throw new WechatConfigException("配置异常，没有配置appid或者appsecret");
            return option;
        }

        private static WechatAccessOption GetSectionOption(IConfigurationSection section)
        {
            if (!section.Exists())
                throw new WechatConfigException($"节点{section.Key}不存在，请检查配置信息");
            var option = section.Get<WechatAccessOption>();
            if (option == null)
                throw new WechatConfigException($"节点{section.Key}配置错误，请检查配置信息");
            return option;
        }

        /// <summary>
        /// 添加基础消息类进入单例模式
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configMessageActions"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        private static IServiceCollection AddBasicMessage(IServiceCollection services, Action<BasicMessage> configMessageActions, WechatAccessOption option)
        {
            AddWechatAccessTokenManager(services, option);
            var tokenManager = services.First(t => t.ServiceType == typeof(WechatAccessTokenManager)).ImplementationInstance as WechatAccessTokenManager;
            if (tokenManager == null)
                throw new WechatException("在此之前没有添加WechatAccessTokenManager，请添加WechatAccessTokenManager后在添加WechatTemplateMessage单例");
            var message = new BasicMessage(option, tokenManager);
            configMessageActions?.Invoke(message);
            services.AddSingleton(message);
            return services;
        }

        /// <summary>
        /// 添加模板处理工具类
        /// </summary>
        /// <param name="services"></param>
        /// <param name="option"></param>
        /// <returns></returns> 
        private static IServiceCollection AddWechatTemplateMessage(this IServiceCollection services, WechatAccessOption option, Action<string, AccessTokenInfo> refreshAction, Action<string> evictionedAction)
        {
            AddWechatAccessTokenManager(services, option);
            var tokenManager = services.First(t => t.ServiceType == typeof(WechatAccessTokenManager)).ImplementationInstance as WechatAccessTokenManager;
            if (tokenManager == null)
                throw new WechatException("在此之前没有添加WechatAccessTokenManager，请添加WechatAccessTokenManager后在添加WechatTemplateMessage单例");
            tokenManager.RefreshTokened += (appid, token) =>
            {
                refreshAction?.Invoke(appid, token);
            };
            tokenManager.TokenEvictioned += (appid) =>
            {
                evictionedAction?.Invoke(appid);
            };

            services.AddSingleton(new TemplateMessageProvider(option, tokenManager));
            return services;
        }

        /// <summary>
        /// 添加token管理工具，一般都不用添加，其他方法会自动检测并且自主添加的
        /// </summary>
        /// <param name="services"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        private static IServiceCollection AddWechatAccessTokenManager(IServiceCollection services, WechatAccessOption option)
        {
            AddAccessTokenManager(services);
            if (services.Any(t => t.ServiceType == typeof(WechatAccessTokenManager))) return services;
            var cacheManager = services.First(t => t.ServiceType == typeof(AccessTokenCacheManager)).ImplementationInstance as AccessTokenCacheManager;
            services.AddSingleton(new WechatAccessTokenManager(cacheManager, option));
            return services;
        }

        #endregion 

        private static void AddWechatAccessTokenManager(IServiceCollection services, string appid, string appsecret)
        {
            AddWechatAccessTokenManager(services, new WechatAccessOption(appid, appsecret));
        }

        private static void AddAccessTokenManager(IServiceCollection services)
        {
            if (services.Any(t => t.ServiceType == typeof(AccessTokenCacheManager))) return;
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            services.AddSingleton(memoryCache);
            //services.AddSingleton<AccessTokenCacheManager>();
            services.AddSingleton(new AccessTokenCacheManager(memoryCache, new LoggerFactory()));
        }
    }
}
