
using Contracts.Commands;
using Contracts.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Orders.Data;
using Orders.Domain;
using Orders.Service;
using OrdersApi.Consumers;
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
                // Replace the incorrect line with the correct AutoMapper configuration
               // builder.Services.AddAutoMapper(cfg => cfg.AddProfile<OrderProfileMapping>());
            });

            builder.Services.AddAutoMapper(cfg=>
            {
                
            }, typeof(OrderProfileMapping));
            builder.Services.AddDbContext<OrderContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            builder.Services.AddHttpClient<IProductStockServiceClient, ProductStockServiceClient>();


            builder.Services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();
                
                x.AddRequestClient<VerifyOrder>();

                x.AddConsumer<OrderReceivedConsumer>();
                x.AddConsumer<VerifyOrderConsumer>();
                x.UsingRabbitMq((context, cfg) =>
                {

                    cfg.ReceiveEndpoint("order-received-notification", e =>
                    {
                        e.ConfigureConsumer<OrderReceivedConsumer>(context);
                    });

                    cfg.ConfigureEndpoints(context);
                });

            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "My API",
                    Version = "v1"
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
                }); 
                
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
