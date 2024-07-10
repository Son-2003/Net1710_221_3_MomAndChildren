using Microsoft.EntityFrameworkCore;
using MomAndChildren.Data.Base;
using MomAndChildren.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomAndChildren.Data.Repositories
{
    public class CustomerRepository : GenericRepository<Customer>
    {
        public CustomerRepository()
        {
        }

        public CustomerRepository(Net1710_221_3_MomAndChildrenContext context) => _context = context;

        //public async Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(int customerId)
        //{
        //    return await _context.Orders.Where(o => o.CustomerId == customerId).ToListAsync();
        //}
    }
}
