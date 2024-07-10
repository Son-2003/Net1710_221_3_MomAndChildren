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

namespace MomAndChildren.RazorWebApp.Pages.OrderPage
{
    public class EditModel : PageModel
    {
        private readonly IOrderBusiness business;
        private readonly ICustomerBusiness customer;

        public EditModel()
        {
            business ??= new OrderBusiness();
            customer ??= new CustomerBusiness();
        }

        [BindProperty]
        public Order Order { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await business.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            ViewData["CustomerId"] = new SelectList((List<Customer>)customer.GetCustomersAsync().Result.Data, "CustomerId", "CustomerId");
            Order = (Order)order.Data;
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
                await business.UpdateOrder(Order);
            }
            catch (Exception ex)
            {

            }

            return RedirectToPage("./Index");
        }

        //private bool OrderExists(int id)
        //{
        //  return (_context.Orders?.Any(e => e.OrderId == id)).GetValueOrDefault();
        //}
    }
}
