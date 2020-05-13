using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace TemplateJob
{
    public class JobService : IHostedService
    {
        private readonly AppSetting _setting;
        public JobService(AppSetting setting)
        {
            _setting = setting;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            ScheduledWork.Start(_setting);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Timed Background Service is stopping.2");
            return Task.CompletedTask;
        }
    }
}
