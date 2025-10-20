
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Orders.Data;
using Orders.Domain;
using Orders.Service;
using OrdersApi.Infrastructure;
using OrdersApi.Service.Clients;
using OrdersApi.Services;

namespace OrdersApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
            });

           /// builder.Services.AddAutoMapper(typeof(OrderProfileMapping).Assembly);
            builder.Services.AddDbContext<OrderContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            builder.Services.AddHttpClient<IProductStockServiceClient, ProductStockServiceClient>();


            builder.Services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();

                x.UsingRabbitMq((context, cfg) =>
                {

                    cfg.ReceiveEndpoint("order-created", e =>
                    {
                        e.UseMessageRetry(i =>
                        {
                            i.Immediate(2);
                        });
                    });

                    cfg.ConfigureEndpoints(context);
                });

            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    serviceScope.ServiceProvider.GetService<OrderContext>().Database.EnsureCreated();
                }
            }

            // app.UseHttpsRedirection();

            //app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
