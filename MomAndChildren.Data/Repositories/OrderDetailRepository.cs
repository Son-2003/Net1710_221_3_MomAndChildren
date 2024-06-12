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
    public class OrderDetailRepository : GenericRepository<OrderDetail>
    {
        public OrderDetailRepository() { }
        public OrderDetailRepository(Net1710_221_3_MomAndChildrenContext context) => _context = context;

        public async Task<List<OrderDetail>> GetAllAsyncOrderDetails()
        {
            return await _context.OrderDetails.Include("Product").ToListAsync();
        }
    }
}
