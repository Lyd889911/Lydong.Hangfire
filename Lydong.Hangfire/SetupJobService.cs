using Hangfire;
using Lydong.Hangfire.Symbols;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lydong.Hangfire
{
    public class SetupJobService : BackgroundService
    {
        private readonly IServiceScopeFactory _spf;
        public SetupJobService(IServiceScopeFactory spf)
        {
            _spf = spf;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _spf.CreateScope();
            var _jobs = scope.ServiceProvider.GetServices<IJob>();
            foreach (var job in _jobs)
            {
                Type type = job.GetType();
                //获取Job方法
                MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public);
                foreach (var method in methods)
                {
                    var jobAttr = method.GetCustomAttribute<JobAttribute>();
                    if (jobAttr != null && method.GetParameters().Length == 0)
                    {
                        string jobName = $"{type.FullName}.{method.Name}";

                        var option = new RecurringJobOptions();
                        option.TimeZone = TimeZoneInfo.FindSystemTimeZoneById("China Standard Time");

                        RecurringJob.AddOrUpdate(jobName, () => JobCallback(type.FullName, method.Name), jobAttr.Cron, option);
                    }
                }
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// 作业回调
        /// </summary>
        public void JobCallback(string typeFullName,string methodName)
        {
            using var scope = _spf.CreateScope();
            var logger = scope.ServiceProvider.GetService<ILogger<SetupJobService>>();
            try
            {
                var job = scope.ServiceProvider.GetServices<IJob>().Where(j => j.GetType().FullName == typeFullName).First();
                job.GetType().GetMethod(methodName)?.Invoke(job, null);
                logger?.LogInformation($"{typeFullName}.{methodName}执行完成");
            }
            catch (Exception ex)
            {
                logger?.LogError($"{typeFullName}.{methodName}执行失败，{ex}");
            }

        }
    }
}
