using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MomAndChildren.Business;
using MomAndChildren.Data.Models;

namespace MomAndChildren.RazorWebApp.Pages.PaymentPage
{
    public class EditModel : PageModel
    {
        private readonly IPaymentBusiness paymentBusiness;
        private readonly IOrderBusiness orderBusiness;

        public EditModel()
        {
            orderBusiness ??= new OrderBusiness();
            paymentBusiness ??= new PaymentBusiness();
        }

        [BindProperty]
        public Payment Payment { get; set; } = default!;
        public String Message {get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await paymentBusiness.GetPaymentByIdAsync((int)id);
            if (payment == null)
            {
                return NotFound();
            }

            PopulateViewData();
            Payment = (Payment)payment.Data;

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                PopulateViewData();

                return Page();
            }



            try
            {
                var result = await paymentBusiness.UpdatePayment(Payment);
                PopulateViewData();
                Message = result.Message;
            }
            catch (Exception ex)
            {
                PopulateViewData();
                Message = "Error updating payment";
            }

            return Page();
        }

        private void PopulateViewData()
        {
            ViewData["StatusOptions"] = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "IsPaid" },
                new SelectListItem { Value = "0", Text = "OnHold" }
            };

            var orders = orderBusiness.GetOrdersAsync().Result.Data as List<Order>;
            ViewData["OrderId"] = new SelectList(orders, "OrderId", "OrderId");
        }

        //private bool PaymentExists(int id)
        //{
        //  return (_context.PaymentHistories?.Any(e => e.PaymentId == id)).GetValueOrDefault();
        //}
    }
}
