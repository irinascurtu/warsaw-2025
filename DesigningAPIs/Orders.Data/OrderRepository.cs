﻿using Microsoft.EntityFrameworkCore;
using Orders.Domain;
using Orders.Domain.Entities;

namespace Orders.Data
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderContext _context;

        public OrderRepository(OrderContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .ToListAsync();
        }

        public async Task<Order> GetOrderAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Order> GetOrderAsync(Guid orderId)
        {
            return await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public async Task<Order> AddOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }


        public async Task<int> UpdateOrderAsync(Order order)
        {
            _context.Entry(order).State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> OrderExistsAsync(int id)
        {
            return await _context.Orders.AnyAsync(e => e.Id == id);
        }
    }
}
