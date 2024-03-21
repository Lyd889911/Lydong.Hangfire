using Hangfire.Redis.StackExchange;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangfire.MemoryStorage;
using Microsoft.Extensions.Logging;
using System.Reflection;
using Hangfire.SqlServer;
using Lydong.Hangfire;
using Lydong.Hangfire.Symbols;
using Microsoft.AspNetCore.Builder;
using Hangfire.Dashboard.BasicAuthorization;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class HangfireExpansions
    {
        public static void AddLydongHangfire(this IServiceCollection services, Action<LydongHangfireOption> optionAction=null)
        {
            LydongHangfireOption option = new LydongHangfireOption();
            if(optionAction != null)
                optionAction(option);

            if (!option.IsEnable)
                return;

            if (option.Storage == HangfireStorageType.Redis)
            {
                var redisOption = new RedisStorageOptions
                {
                    Prefix = "hangfire:",
                    Db = 0
                };
                services.AddHangfire(configuration => configuration.UseRedisStorage(option.ConnectionString, redisOption));
            }
            else if (option.Storage == HangfireStorageType.SqlServer)
            {
                services.AddHangfire(configuration => configuration.UseSqlServerStorage(option.ConnectionString));
            }
            else if (option.Storage == HangfireStorageType.Memory)
            {
                services.AddHangfire(configuration => configuration.UseMemoryStorage());
            }


            services.AddHangfireServer(backgroundJobServerOptions =>
            {
                backgroundJobServerOptions.SchedulePollingInterval = new TimeSpan(option.Heartbeat * TimeSpan.TicksPerSecond);
            });

            RegisterJob(services);

            services.AddHostedService<SetupJobService>();

        }

        public static void UseLydongHangfire(this IApplicationBuilder app, Action<LydongHangfireOption> optionAction=null)
        {
            LydongHangfireOption option = new LydongHangfireOption();
            if (optionAction != null)
                optionAction(option);

            var doptions = new DashboardOptions()
            {
                Authorization = new[]
                    {
                        new BasicAuthAuthorizationFilter(new BasicAuthAuthorizationFilterOptions(){
                            RequireSsl = false,
                            SslRedirect = false,
                            LoginCaseSensitive = true,
                            Users = new[]
                            {
                                new BasicAuthAuthorizationUser
                                {
                                    Login = option.LoginUserName,
                                    PasswordClear =  option.LoginPassword
                                }
                            }
                        })
                    }
            };
            app.UseHangfireDashboard("/hangfire", doptions);
        }

        private static void RegisterJob(IServiceCollection services)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                Type[] types = assembly.GetTypes();
                foreach (Type type in types)
                {
                    if (!type.IsAbstract && !type.IsInterface && type.IsAssignableTo(typeof(IJob)))
                    {
                        services.AddScoped(typeof(IJob), type);
                    }
                }
            }
        }

    }
}
