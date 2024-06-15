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
            return await _context.OrderDetails.OrderByDescending(tmp => tmp.OrderDetailId).Include("Product").Include("Order").ToListAsync();
        }

        public async Task<OrderDetail> GetOrderDetailByIdAsync(int id)
        {
            return await _context.OrderDetails.Where(tmp => tmp.OrderDetailId == id).Include("Product").Include("Order").FirstAsync();
        }
    }
}
