# TestTask
Реализованы интерфейсы IUserService, IOrderService, для низ настроено DI
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

Также произведена проверка на корректность заданным условиям

1.
```
{
  "id": 7,
  "email": "user7@gmail.com",
  "status": 0,
  "orders": null
}
```
2.
```
[
  {
    "id": 1,
    "email": "user1@gmail.com",
    "status": 0,
    "orders": null
  },
  {
    "id": 7,
    "email": "user7@gmail.com",
    "status": 0,
    "orders": null
  }
]
```
3.
```
{
  "id": 4,
  "productName": "Cabbage",
  "price": 7,
  "quantity": 2,
  "userId": 2,
  "createdAt": "2023-08-08T12:00:00",
  "status": 3,
  "user": null
}
```
4.
```
[
  {
    "id": 9,
    "productName": "Watermelon",
    "price": 100,
    "quantity": 1,
    "userId": 4,
    "createdAt": "2024-03-08T12:00:00",
    "status": 3,
    "user": null
  },
  {
    "id": 4,
    "productName": "Cabbage",
    "price": 7,
    "quantity": 2,
    "userId": 2,
    "createdAt": "2023-08-08T12:00:00",
    "status": 3,
    "user": null
  },
  {
    "id": 14,
    "productName": "Pumpkin",
    "price": 50,
    "quantity": 1,
    "userId": 7,
    "createdAt": "2021-06-11T12:00:00",
    "status": 1,
    "user": null
  },
  {
    "id": 6,
    "productName": "Carrot",
    "price": 9,
    "quantity": 5,
    "userId": 2,
    "createdAt": "2020-09-10T12:00:00",
    "status": 3,
    "user": null
  },
  {
    "id": 10,
    "productName": "Garlic",
    "price": 8,
    "quantity": 12,
    "userId": 4,
    "createdAt": "2019-05-14T12:00:00",
    "status": 3,
    "user": null
  },
  {
    "id": 5,
    "productName": "Onion",
    "price": 8,
    "quantity": 6,
    "userId": 2,
    "createdAt": "2019-01-01T12:00:00",
    "status": 2,
    "user": null
  },
  {
    "id": 3,
    "productName": "Cucumber",
    "price": 5,
    "quantity": 10,
    "userId": 1,
    "createdAt": "2010-06-01T12:00:00",
    "status": 1,
    "user": null
  },
  {
    "id": 11,
    "productName": "Potato",
    "price": 3,
    "quantity": 100,
    "userId": 7,
    "createdAt": "2010-01-01T12:00:00",
    "status": 1,
    "user": null
  },
  {
    "id": 12,
    "productName": "Carrot",
    "price": 9,
    "quantity": 15,
    "userId": 7,
    "createdAt": "2006-09-01T12:00:00",
    "status": 2,
    "user": null
  },
  {
    "id": 2,
    "productName": "Lemon",
    "price": 30,
    "quantity": 2,
    "userId": 1,
    "createdAt": "2004-05-31T12:00:00",
    "status": 3,
    "user": null
  },
  {
    "id": 15,
    "productName": "Watermelon",
    "price": 100,
    "quantity": 12,
    "userId": 7,
    "createdAt": "2003-12-21T12:00:00",
    "status": 3,
    "user": null
  },
  {
    "id": 1,
    "productName": "Apple",
    "price": 10,
    "quantity": 130,
    "userId": 1,
    "createdAt": "2003-07-28T12:00:00",
    "status": 0,
    "user": null
  },
  {
    "id": 13,
    "productName": "Onion",
    "price": 8,
    "quantity": 15,
    "userId": 7,
    "createdAt": "2003-05-30T12:00:00",
    "status": 3,
    "user": null
  },
  {
    "id": 8,
    "productName": "Orange",
    "price": 45,
    "quantity": 5,
    "userId": 4,
    "createdAt": "2003-03-03T12:00:00",
    "status": 3,
    "user": null
  }
]
```
