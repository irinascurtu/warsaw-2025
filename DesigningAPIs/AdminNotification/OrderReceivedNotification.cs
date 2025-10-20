using Contracts.Events;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminNotification
{
    public class OrderReceivedNotification : IConsumer<OrderReceived>
    {
        public Task Consume(ConsumeContext<OrderReceived> context)
        {
            Console.WriteLine("OrderReceivedConsumer - received");
            Console.WriteLine(context.ReceiveContext.InputAddress);
            Console.WriteLine($"I was notified about an order with OrderId{context.Message.OrderId}");
            return Task.CompletedTask;
        }
    }
}
