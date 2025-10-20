using Contracts.Events;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminNotification
{
    public class OrderReceivedConsumer: IConsumer<OrderReceived>
    {
        public Task Consume(ConsumeContext<OrderReceived> context)
        {
            

            Console.WriteLine("OrderReceivedConsumer- received");
            return Task.CompletedTask;
        }
    }
}
