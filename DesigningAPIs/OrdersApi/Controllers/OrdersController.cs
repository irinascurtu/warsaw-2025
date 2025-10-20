using AutoMapper;
using Contracts.Commands;
using Contracts.Events;
using Contracts.Models;
using Contracts.Responses;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Orders.Domain.Entities;
using Orders.Service;
using OrdersApi.Service.Clients;

namespace OrdersApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IProductStockServiceClient _productStockServiceClient;
        private readonly ISendEndpointProvider sendEndpointProvider;//used to send commands
        private readonly IPublishEndpoint publishEndpoint;
        private readonly IRequestClient<VerifyOrder> requestClient;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService orderService,
            IProductStockServiceClient productStockServiceClient,
            ISendEndpointProvider sendEndpointProvider,
            IPublishEndpoint publishEndpoint, IRequestClient<VerifyOrder> requestClient
            )
        {
            _orderService = orderService;
            _productStockServiceClient = productStockServiceClient;
            this.sendEndpointProvider = sendEndpointProvider;
            this.publishEndpoint = publishEndpoint;
            this.requestClient = requestClient;
            //   _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(OrderModel model)
        {
            //var orderToAdd = _mapper.Map<Order>(model);
            //var createdOrder = await _orderService.AddOrderAsync(orderToAdd);
            var orderId = Guid.NewGuid();

            var sendEndpoint = await sendEndpointProvider.GetSendEndpoint(new Uri("queue:create-order"));


            await sendEndpoint.Send(model);

            await publishEndpoint.Publish<OrderReceived>(new OrderReceived()
            {
                OrderId = orderId
            });

            return Accepted();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {

            var response = await requestClient.GetResponse<OrderResult, OrderNotFoundResult>(
              new VerifyOrder { Id = id });

            if (response.Is(out Response<OrderResult> incomingMessage))
            {
                return Ok(incomingMessage.Message);
            }

            if (response.Is(out Response<OrderNotFoundResult> notfound))
            {
                return NotFound(notfound.Message.ErrorMessage);
            }

            return BadRequest();
        }
    }
}
