﻿using Orders.Domain.Entities;

namespace Orders.Data
{
    public interface IOrderRepository
    {
        Task<Order> AddOrderAsync(Order order);
        Task DeleteOrderAsync(int id);
        Task<Order> GetOrderAsync(int id);
        Task<Order> GetOrderAsync(Guid orderId);
        Task<IEnumerable<Order>> GetOrdersAsync();
        Task<bool> OrderExistsAsync(int id);
      
        Task<int> SaveChangesAsync();
        Task<int> UpdateOrderAsync(Order order);
    }
}