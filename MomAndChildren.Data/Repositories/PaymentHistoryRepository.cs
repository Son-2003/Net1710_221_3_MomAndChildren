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
    public class PaymentHistoryRepository : GenericRepository<Payment>
    {
        public PaymentHistoryRepository() { }
        public PaymentHistoryRepository(Net1710_221_3_MomAndChildrenContext context) => _context = context;
        public async Task<List<Payment>> GetPaymentHistoryListByCustomerId(int id)
        {
            return await _context.Payments
                .Where(x => x.Order.CustomerId == id)
                .OrderByDescending(x => x.PaymentId)
                .ToListAsync();
        }

    }
}
