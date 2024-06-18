using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MomAndChildren.Business;
using MomAndChildren.Data.Models;

namespace MomAndChildren.RazorWebApp.Pages.CustomerPage
{
    public class CreateModel : PageModel
    {
        private readonly ICustomerBusiness business;
        //private readonly ICustomerBusiness customer;

        public CreateModel()
        {
            business ??= new CustomerBusiness();
            //customer ??= new CustomerBusiness();
        }

        public IActionResult OnGet()
        {
            //ViewData["CustomerId"] = new SelectList((List<Customer>)customer.GetCustomersAsync().Result.Data, "CustomerId", "CustomerId");
            return Page();
        }

        [BindProperty]
        public Customer Customer { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await business.CreateCustomer(Customer);

            return RedirectToPage("./Index");
        }
    }
}
