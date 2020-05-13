using System;
using Quartz;
using Quartz.Impl;
using TemplateJob.Creators;

namespace TemplateJob
{
    public class ScheduledWork
    {
        public static async void Start(AppSetting settings)
        {
            var schedular = await StdSchedulerFactory.GetDefaultScheduler();

            await schedular.Start();

            var jobCreator = JobBuilder.Create<JobCreator>()
                .Build();

            jobCreator.JobDataMap.Put("serviceTask", settings);

            var jobTrigger = TriggerBuilder.Create()
                .WithIdentity("createJobTrigger", "NameJobs")
                .StartNow()
                .WithSimpleSchedule(x => x.WithInterval(settings.AutoJobInterval).RepeatForever())
                .Build();

            try
            {
                await schedular.ScheduleJob(jobCreator, jobTrigger);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
    }
}
