using Contracts.Commands;
using Contracts.Responses;
using MassTransit;
using MassTransit.Testing;
using Orders.Domain.Entities;
using Orders.Service;
using OrdersApi.Services;

namespace OrdersApi.Consumers
{
    public class VerifyOrderConsumer : IConsumer<VerifyOrder>
    {
        private readonly IOrderService service;

        public VerifyOrderConsumer(IOrderService service)
        {
            this.service = service;
        }

        public async Task Consume(ConsumeContext<VerifyOrder> context)
        {
            var existingOrder = await service.GetOrderAsync(context.Message.Id);

            if (context.Message.Id == 1)
            {
                await context.RespondAsync<OrderResult>(new
                {
                    Id = context.Message.Id,
                    OrderDate = DateTime.Now,
                    Status = OrderStatus.Pending
                });
            }
            else
            {
                await context.RespondAsync<OrderNotFoundResult>(
                    new OrderNotFoundResult()
                    {
                        ErrorMessage = "Order not found"
                    });

            }

        }
    }
}
