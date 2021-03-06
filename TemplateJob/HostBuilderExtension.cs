﻿using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Topshelf;

namespace TemplateJob
{
    public static class HostBuilderExtension
    {
        public static IHostBuilder UseTopshelfLifetime(this IHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IHostLifetime, TopshelfLifetime>();
            });
        }

        /// <summary>
        /// Builds and run the host as a Topshelf service.
        /// </summary>
        /// <param name="hostBuilder"></param>
        /// <param name="options">Configuration of the topshelf service.</param>
        /// <returns></returns>
        public static TopshelfExitCode RunAsTopshelfService(this IHostBuilder hostBuilder, Action<Topshelf.HostConfigurators.HostConfigurator> configureTopshelfHost)
        {
            if (configureTopshelfHost == null)
                throw new ArgumentNullException(nameof(configureTopshelfHost));

            hostBuilder.UseTopshelfLifetime();

            var rc = HostFactory.Run(x =>
            {
                configureTopshelfHost(x);
                x.Service<IHost>((Action<Topshelf.ServiceConfigurators.ServiceConfigurator<IHost>>)(s =>
                {
                    s.ConstructUsing(() => hostBuilder.Build());
                    s.WhenStarted(service =>
                    {
                        service.Start();
                    });
                    s.WhenStopped(service =>
                    {
                        service.StopAsync();
                    });
                }));
            });

            return rc;
        }
    }
}
