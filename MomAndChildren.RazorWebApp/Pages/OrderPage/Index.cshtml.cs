using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MomAndChildren.Business;
using MomAndChildren.Data.Models;

namespace MomAndChildren.RazorWebApp.Pages.OrderPage
{
    public class IndexModel : PageModel
    {
        private readonly IOrderBusiness business;
        //private readonly ICustomerBusiness customer;

        public IndexModel()
        {
            business ??= new OrderBusiness();
            //customer ??= new CustomerBusiness();
        }

        public IEnumerable<Order> Order { get; set; } = default!;

        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public string? Description { get; set; }

        public async Task<IActionResult> OnGetAsync(int currentPage = 1, string searchTerm = "")
        {
            var result = await business.GetOrdersAsync();

            //Paging
            CurrentPage = currentPage;
            int pageSize = 5;

            //Search
            if (result != null && result.Status > 0 && result.Data != null)
            {
                Order = result.Data as List<Order>;
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                result = await business.SearchByCustomerName(searchTerm);
                Order = result.Data as List<Order>;

                return Page();
            }


            int totalOrderDetail = Order.Count();
            TotalPages = (int)Math.Ceiling((double)totalOrderDetail / pageSize);

            Order = Order.Skip((currentPage - 1) * pageSize).Take(pageSize);

            return Page();
        }
    }
}
