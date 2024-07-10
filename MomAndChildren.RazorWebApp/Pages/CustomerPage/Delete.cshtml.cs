using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MomAndChildren.Business;
using MomAndChildren.Data.Models;

namespace MomAndChildren.RazorWebApp.Pages.CustomerPage
{
    public class DeleteModel : PageModel
    {
        private readonly ICustomerBusiness business;

        public DeleteModel()
        {
            business = new CustomerBusiness();
        }

        [BindProperty]
        public Customer Customer { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await business.GetCustomerByIdAsync(id);

            if (customer == null)
            {
                return NotFound();
            }
            else
            {
                Customer = customer.Data as Customer;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var category = await business.DeleteCustomer(id);

            return RedirectToPage("./Index");
        }
    }
}
