using Microsoft.Extensions.Logging;
using Mk.DemoB.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;

namespace Mk.DemoB.BackgroundJobs
{

    // 需要用 IBackgroundJobManager 把作业加入到队列中

    public class SimpleAbpJob : BackgroundJob<SimpleAbpArgs>, ITransientDependency
    {
        public SimpleAbpJob()
        {

        }

        public override void Execute(SimpleAbpArgs args)
        {
            var msg = $"Abp 后台任务：CurrentTime:{ DateTime.Now}, Hello World!,JobCreatime:{args.CreateTime}";

            Console.WriteLine(msg);

            Logger.LogInformation(msg);
        }

    }
}
