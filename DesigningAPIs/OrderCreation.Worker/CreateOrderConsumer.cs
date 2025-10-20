using AutoMapper;
using Contracts.Commands;
using Contracts.Events;
using Contracts.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Orders.Data;
using Orders.Domain.Entities;
using Orders.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderCreation.Worker
{
    public class CreateOrderConsumer : IConsumer<OrderModel>
    {
        private readonly IOrderService orderService;
        private readonly IOrderRepository orderRepository;
        private readonly IMapper mapper;

        public CreateOrderConsumer(IOrderService orderService, IMapper mapper, IOrderRepository orderRepository)
        {
            this.orderService = orderService;
            this.mapper = mapper;
            this.orderRepository = orderRepository;
        }


        public async Task Consume(ConsumeContext<OrderModel> context)
        {
            Console.WriteLine($"I got a command to create an order:{context.Message}");
            //mapping from Message to an order object
            var orderToAdd = mapper.Map<Order>(context.Message);

            var orderReceived = context.Publish(new OrderReceived()//maybe an admin does something
            {
                CreatedAt = orderToAdd.OrderDate,
                OrderId = orderToAdd.OrderId
            });
            var savedOrder = await orderService.AddOrderAsync(orderToAdd);



            var notifyOrderCreated = context.Publish(new OrderCreated()
            {
                CreatedAt = savedOrder.OrderDate,
                OrderId = savedOrder.OrderId
            });

            try
            {
                await orderService.SaveChangesAsync();
            }
            catch (DbUpdateException exception)
            {

            }

            await Task.CompletedTask;
        }
    }
}
