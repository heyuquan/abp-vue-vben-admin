using Microsoft.Extensions.Logging;
using Mk.DemoB.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Timing;

namespace Mk.DemoB.BackgroundJobs
{
    // 后台作业
    // 需要用 IBackgroundJobManager 把作业加入到队列中

    public class SimpleAbpJob : BackgroundJob<SimpleAbpArgs>, ITransientDependency
    {
        private readonly Clock _clock;
        public SimpleAbpJob(
            Clock clock
            )
        {
            _clock = clock;
        }

        public override void Execute(SimpleAbpArgs args)
        {
            var msg = $"Abp 后台任务：CurrentTime:{ _clock.Now}, Hello World!,JobCreatime:{args.CreateTime}";

            Console.WriteLine(msg);

            Logger.LogInformation(msg);
        }

    }
}
