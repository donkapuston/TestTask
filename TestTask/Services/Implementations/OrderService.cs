using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Enums;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Order> GetOrder()
        {
            var newestOrder = await _context.Orders
                .Where(order => order.Quantity > 1)
                .OrderByDescending(order => order.CreatedAt)
                .FirstOrDefaultAsync();

            if (newestOrder!=null)
            {
                return newestOrder;
            }
            else
            {
                return null;
            }         
        }

        public async Task<List<Order>> GetOrders()
        {
            var activeUsers = await _context.Users
                .Where(u => u.Status == UserStatus.Active)
                .Select(u => u.Id)
                .ToListAsync();

            if (activeUsers!=null)
            {
                var orders = await _context.Orders
                .Where(o => activeUsers.Contains(o.UserId))
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

                if (orders!=null)
                {
                    return orders;
                }
                else
                {
                    return null;
                }                
            }
            else
            {
                return null;
            }        
        }
    }
}
