using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Enums;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
                _context = context;
        }

        public async Task<User> GetUser()
        {
            var userMaxAlmount = await _context.Orders
                 .Where(order => order.CreatedAt.Year == 2003 && order.Status == OrderStatus.Delivered)
                 .GroupBy(order => order.UserId)
                 .Select(group => new
                 {
                     UserId = group.Key,
                     TotalAmount = group.Sum(order => order.Price * order.Quantity)
                 })
                 .OrderByDescending(result => result.TotalAmount)
                 .Select(result => result.UserId)
                 .FirstOrDefaultAsync();

            if(userMaxAlmount != null)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u=> u.Id == userMaxAlmount);
                if(user != null)
                {
                    return user;
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

        public async Task<List<User>> GetUsers()
        {

            var orders = await _context.Orders
                .Where(order => order.Status == OrderStatus.Paid && order.CreatedAt.Year == 2010)
                .Select(order => order.UserId)
                .Distinct()
                .ToListAsync();

            if (orders!=null)
            {
                var users = await _context.Users
                .Where(user => orders.Contains(user.Id))
                .ToListAsync();

                if (users != null)
                {
                    return users;
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
