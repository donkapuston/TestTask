# TestTask
Реализованы интерфейсы IUserService, IOrderService, для них настроено DI
# Приложение содержит следующие функциональные возможности:

    1. Возвращать пользователя с максимальной общей суммой товаров, доставленных в 2003
 ```
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
   ```
    2. Возвращать пользователей у которых есть оплаченные заказы в 2010
```
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
```
    3. Возвращать самый новый заказ, в котором больше одного предмета.
```
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
```
    4. Возвращать заказы от активных пользователей, отсортированные по дате создания.
```
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
```

