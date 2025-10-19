using AutoMapper;
using Contracts.Events;
using MassTransit;
using Contracts.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentProcessor.Services;

namespace PaymentProcessor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IPublishEndpoint publishEndpoint;

        public PaymentController( IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            this.mapper = mapper;
            this.publishEndpoint = publishEndpoint;
        }

        [HttpPost("/pay")]
        public async Task<ActionResult> Post(PayOrderModel model)
        {

            var orderPaidEvent = mapper.Map<OrderPaid>(model);

            //set it as a gift
            orderPaidEvent.IsGift = true;
            await publishEndpoint.Publish(orderPaidEvent);

            //transform to call external
            //await external call to WackyPayments. if successfull



            //save payment trace in PaymentProcessorDB either as failed or successfull
            //Ok ();
            //NotOK()
            //}
            //validate
            //notify OrderApi and change OrderStatus in a PUT- or call ORders Endpoint-external servie


            //
            return Ok("Payment Processor API");
        }
    }
}
