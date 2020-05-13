using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TemplateJob
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new HostBuilder()
                .ConfigureHostConfiguration(configHost => { configHost.SetBasePath(Directory.GetCurrentDirectory()); })
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    configApp.AddJsonFile("appsetting.json", true);
                })
                .ConfigureServices((hostContext, services) =>
                {

                    var settings = hostContext.Configuration.Get<AppSetting>();
                    services.AddSingleton(settings);
                    services.AddLogging();
                    services.AddSingleton<IHostLifetime, TopshelfLifetime>();

                })
                .ConfigureLogging((hostContext, configLogging) =>
                {
                    configLogging.AddConsole();
                    configLogging.AddDebug();
                });


            builder.RunAsTopshelfService(hc =>
            {
                hc.SetServiceName("MEGAJobs.MailSend");
                hc.SetDisplayName("MEGAJobs MailSend");
                hc.SetDescription("Send Emails for MEGA.");
            });
        }
    }
}
