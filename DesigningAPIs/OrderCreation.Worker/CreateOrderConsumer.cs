using Contracts.Commands;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderCreation.Worker
{
    public class CreateOrderConsumer : IConsumer<CreateOrder>
    {
        public CreateOrderConsumer()
        {
            
        }

        public Task Consume(ConsumeContext<CreateOrder> context)
        {
            //map the the message
            //save it in database
            //notify the OrderCreated event

            Console.WriteLine("I am consuming from CreateOrderConsumer");
            return Task.CompletedTask;
        }
    }
}
