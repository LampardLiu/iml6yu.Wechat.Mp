using iml6yu.Wechat.Mp.Message;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WorkerService1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddMemoryCache();
                    services.UseWechatTemplateMessage(o =>
                    {
                        o.AppId = "wxc5f302e2410fb320";
                        o.AppSecret = "46b0526f889e452b5787ddc226656a16";
                    }, (app, token) => {
                        ;
                    }, (app) => {
                        ;
                    });
                    services.AddHostedService<Worker>();
                });
    }
}
