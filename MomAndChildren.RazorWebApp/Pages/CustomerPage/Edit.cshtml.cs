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

namespace MomAndChildren.RazorWebApp.Pages.CustomerPage
{
    public class EditModel : PageModel
    {
        private readonly ICustomerBusiness business;
        //private readonly ICustomerBusiness customer;

        public EditModel()
        {
            business ??= new CustomerBusiness();
            //customer ??= new CustomerBusiness();
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

            //ViewData["CustomerId"] = new SelectList((List<Customer>)customer.GetCustomersAsync().Result.Data, "CustomerId", "CustomerId");
            Customer = customer.Data as Customer;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }


            try
            {
                await business.UpdateCustomer(Customer);
            }
            catch (Exception ex)
            {

            }

            return RedirectToPage("./Index");
        }
    }
}
