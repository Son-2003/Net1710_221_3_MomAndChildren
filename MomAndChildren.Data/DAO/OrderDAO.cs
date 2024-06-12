using Microsoft.EntityFrameworkCore;
using MomAndChildren.Data.Base;
using MomAndChildren.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomAndChildren.Data.DAO
{
    public class OrderDAO : BaseDAO<Order>
    {
        public OrderDAO() : base()
        { }
        
            public async Task<List<Order>> GetOrdersByCustomerIdAsync(int customerId)
            {
                return await _dbSet
                    .Where(o => o.CustomerId == customerId)
                    .Include(o => o.Customer)
                    .Include(o => o.OrderDetails)
                    .Include(o => o.Payments)
                    .ToListAsync();
            }
        
    }
}
