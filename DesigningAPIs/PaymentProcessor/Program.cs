
using MassTransit;
using OrdersApi.Infrastructure.Mappings;
using PaymentProcessor.Clients;
using PaymentProcessor.Infrastructure.Mappings;

namespace PaymentProcessor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddAutoMapper(typeof(CheckoutProfileMapping));

            builder.Services.AddHttpClient<IWackyPaymentsClient, WackyPaymentsClient>();
            ///add policies here and test them out
            //.AddHttpMessageHandler<MyCustomHttpMessageHandler>() // Using a custom handler
            //.AddPolicyHandler(GetRetryPolicy()); // using a Polly handler;
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();

                // Step 2: Select a Transport
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.ConfigureEndpoints(context);
                });
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
