using Orders.Domain.Entities;

namespace Contracts.Models
{
    public class OrderModel
    {
        public OrderModel()
        {
            Status = OrderStatus.Accepted;
        }

        public Guid OrderId { get; set; }
        private OrderStatus Status { get; set; }

        //customer things
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        //Shipping things

        public List<OrderItemModel> OrderItems { get; set; }
    }
}
