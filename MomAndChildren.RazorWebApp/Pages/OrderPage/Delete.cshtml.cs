using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MomAndChildren.Business;
using MomAndChildren.Data.Models;

namespace MomAndChildren.RazorWebApp.Pages.OrderPage
{
    public class DeleteModel : PageModel
    {
        private readonly IOrderBusiness business;

        public DeleteModel()
        {
            business ??= new OrderBusiness();
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
            else
            {
                Order = order.Data as Order;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var order = await business.DeleteOrder(id);

            return RedirectToPage("./Index");
        }
    }
}
