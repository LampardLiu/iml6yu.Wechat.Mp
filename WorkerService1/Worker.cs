using iml6yu.Wechat.Mp.Message;
using iml6yu.Wechat.Mp.Message.TemplateMessage;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WorkerService1
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private IMemoryCache cache;
        private TemplateMessageProvider provider;
        private WechatAccessTokenManager wechatAccessToken;

        public AccessTokenCacheManager tokenCacheManager;

        public Worker(ILogger<Worker> logger, IMemoryCache cache, TemplateMessageProvider provider, WechatAccessTokenManager wechatAccessToken, AccessTokenCacheManager tokenCacheManager)
        {
            this.cache = cache;
            _logger = logger;
            this.provider = provider;
            this.wechatAccessToken = wechatAccessToken;
            this.tokenCacheManager = tokenCacheManager;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            wechatAccessToken.GetAccessTokenAsync("wxc5f302e2410fb320", "46b0526f889e452b5787ddc226656a16").Wait();
            return base.StartAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var a = tokenCacheManager.Get("wxc5f302e2410fb320");
                _logger.LogInformation($"[{Newtonsoft.Json.JsonConvert.SerializeObject(a)}]Worker running at: {DateTimeOffset.Now}");
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
    public class AAA
    {
        //–’√˚
        public string Name { get; set; }
        //ƒÍ¡‰
        public int Age { get; set; }
    }
}
