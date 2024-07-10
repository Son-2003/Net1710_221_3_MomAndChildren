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
    public class PaymentRepository : GenericRepository<Payment>
    {
        public PaymentRepository() { }
        public PaymentRepository(Net1710_221_3_MomAndChildrenContext context) => _context = context;
        public async Task<List<Payment>> GetPaymentListByCustomerId(int id)
        {
            return await _context.Payments
                .Where(x => x.Order.CustomerId == id)
                .OrderByDescending(x => x.PaymentId)
                .ToListAsync();
        }
        public bool IsOrderInUse(int orderId)
        {
            return _context.Payments.Any(p => p.Order.OrderId == orderId);
        }

        public async Task<List<Payment>> GetAllPaymentAsync()
        {
            return await _context.Payments.Include(p => p.Order).ToListAsync();
        }

        public void Attach(Payment payment)
        {
            _context.Attach(payment);
        }

        public Payment GetPaymentById(int id)
        {
            return _context.Payments.Include(p => p.Order).FirstOrDefault();
        }
    }
}
