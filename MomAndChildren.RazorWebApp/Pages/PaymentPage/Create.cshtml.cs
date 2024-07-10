using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MomAndChildren.Business;
using MomAndChildren.Data.Models;

namespace MomAndChildren.RazorWebApp.Pages.PaymentPage
{
    public class CreateModel : PageModel
    {
        private readonly IPaymentBusiness paymentBusiness;
        private readonly IOrderBusiness orderBusiness;

        public CreateModel()
        {
            orderBusiness ??= new OrderBusiness();
            paymentBusiness ??= new PaymentBusiness();
        }

        public IActionResult OnGetAsync()
        {
            PopulateViewData();
            return Page();
        }

        [BindProperty]
        public Payment Payment { get; set; } = default!;
        public String Message { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                PopulateViewData();
                return Page();
            }


            try
            {
                var result = await paymentBusiness.CreatePayment(Payment);
                PopulateViewData();
                Message = result.Message;
            }
            catch (Exception ex)
            {
                PopulateViewData();
                Message = "Error creating payment";
            }

            return Page();
            //return RedirectToPage("./Index");
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
    }
}
