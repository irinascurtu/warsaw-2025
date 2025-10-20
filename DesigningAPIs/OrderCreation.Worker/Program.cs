using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orders.Data;
using Orders.Domain;
using Orders.Service;
using OrdersApi.Infrastructure;
using OrdersApi.Services;
using System.Reflection;
using System.Threading.Tasks;

namespace OrderCreation.Worker
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
                    services.AddDbContext<OrderContext>(options =>
                    {
                        options.UseSqlServer(hostContext.Configuration.GetConnectionString("DefaultConnection"));
                        options.EnableSensitiveDataLogging(true);
                    });

                    services.AddScoped<IOrderRepository, OrderRepository>();
                    services.AddScoped<IOrderService, OrderService>();
                    services.AddAutoMapper(cfg =>
                    {

                    }, typeof(OrderProfileMapping));


                    services.AddMassTransit(x =>
                    {
                        x.SetKebabCaseEndpointNameFormatter();

                        var entryAssembly = Assembly.GetEntryAssembly();
                        x.AddConsumers(entryAssembly);
                       
                        x.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.UseMessageRetry(r => r.Immediate(2));

                            cfg.ConfigureEndpoints(context);
                        });
                    });
                });
    }
}
