using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MomAndChildren.Business;
using MomAndChildren.Data.Models;

namespace MomAndChildren.RazorWebApp.Pages.OrderPage
{
    public class CreateModel : PageModel
    {
        private readonly IOrderBusiness business;
        private readonly ICustomerBusiness customer;

        public CreateModel()
        {
            business ??= new OrderBusiness();
            customer ??= new CustomerBusiness();
        }

       

        [BindProperty]
        public Order Order { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            ViewData["CustomerId"] = new SelectList((List<Customer>)customer.GetCustomersAsync().Result.Data, "CustomerId", "CustomerId");
            return Page();
        }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await business.CreateOrder(Order);

            return RedirectToPage("./Index");
        }
    }
}
