using Contracts.Events;
using MassTransit;
using Orders.Domain.Entities;
using Orders.Service;
using Spectre.Console;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace AdminNotification
{
    public class OrderCreatedNotification : IConsumer<OrderCreated>
    {
      
        public async Task Consume(ConsumeContext<OrderCreated> context)
        {
            await Task.Delay(1000);
            Console.WriteLine(context.ReceiveContext.InputAddress);
            AnsiConsole.Write(new Text($"I was just notified of a new order with OrderId:{context.Message.OrderId} , that was created At:{context.Message.CreatedAt}", 
                new Style(foreground: Color.Green)));
            AnsiConsole.WriteLine();
            //Console.WriteLine($"I was just notified of a new order with OrderId:{context.Message.OrderId} , that was created At:{context.Message.CreatedAt}");
            
            string jsonString = JsonSerializer.Serialize(context.Message, new JsonSerializerOptions
            {
                WriteIndented = true,
               
            });
                        

            // Write the JSON string using Spectre.Console's Markup
            AnsiConsole.Write(new Text(jsonString, new Style(foreground: Color.Green)));
            //Update the status of the order to created
            //var existingOrder = await orderService.GetOrderAsync(context.Message.OrderId);

            //if (existingOrder != null)
            //{
            //    existingOrder.Status = OrderStatus.Created;
            //    await orderService.UpdateOrderAsync(existingOrder);
            //   // await orderService.SaveChangesAsync();
            //}
        }
    }
}
