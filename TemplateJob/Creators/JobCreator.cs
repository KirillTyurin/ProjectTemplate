using System;
using System.Threading.Tasks;
using Quartz;

namespace TemplateJob.Creators
{
    [DisallowConcurrentExecution]
    public class JobCreator : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var dataMap = context.MergedJobDataMap;

            try
            {
                Console.WriteLine("Job run");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
