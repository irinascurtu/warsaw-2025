using AutoMapper;
using Contracts.Events;
using Contracts.Models;
using PaymentProcessor.Models;

namespace PaymentProcessor.Infrastructure.Mappings
{
    public class CheckoutProfileMapping : Profile
    {
        public CheckoutProfileMapping()
        {
            CreateMap<PayOrderModel, OrderPaid>();
        }
    }
}
