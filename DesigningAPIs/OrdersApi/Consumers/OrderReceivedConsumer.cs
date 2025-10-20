using Contracts.Events;
using MassTransit;

namespace OrdersApi.Consumers
{
    public class OrderReceivedConsumer : IConsumer<OrderReceived>
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
