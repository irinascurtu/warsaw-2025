using Contracts.Models;
using Orders.Domain.Entities;

namespace Orders.Service
{
    public interface IOrderService
    {
        Task<Order> AddOrderAsync(Order order);
        Task DeleteOrderAsync(int id);
        Task<Order> GetOrderAsync(int id);
        Task<IEnumerable<Order>> GetOrdersAsync();
        Task<int> UpdateOrderAsync(Order order);
        Task<bool> OrderExistsAsync(int id);
        Task AcceptOrder(OrderModel model);
        Task<int> SaveChangesAsync();
        Task<Order> GetOrderAsync(Guid orderId);
    }
}