using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orders.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AdminNotification
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddMassTransit(x =>
                    {
                        x.SetKebabCaseEndpointNameFormatter();

                        services.AddDbContext<OrderContext>(options =>
                        {
                            options.UseSqlServer(hostContext.Configuration.GetConnectionString("DefaultConnection"))
                            .ConfigureWarnings(w => w.Ignore(SqlServerEventId.SavepointsDisabledBecauseOfMARS));
                            
                            options.EnableSensitiveDataLogging(false);

                        });

                        x.AddEntityFrameworkOutbox<OrderContext>(o =>
                        {
                            o.UseSqlServer();
                            o.DisableInboxCleanupService();
                        });

                        x.AddConfigureEndpointsCallback((context, name, cfg) =>
                        {
                            cfg.UseEntityFrameworkOutbox<OrderContext>(context);
                        });

                        var entryAssembly = Assembly.GetEntryAssembly();

                        x.AddConsumers(entryAssembly);
                      
                        x.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.ConfigureEndpoints(context);
                        });
                    });
                });
    }
}
