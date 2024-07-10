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
    public class IndexModel : PageModel
    {
        private readonly ICustomerBusiness business;
        //private readonly ICustomerBusiness customer;

        public IndexModel()
        {
            business ??= new CustomerBusiness();
            //customer ??= new CustomerBusiness();
        }

        public IEnumerable<Customer> Customer { get; set; } = default!;


        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public string? Description { get; set; }

        public async Task<IActionResult> OnGetAsync(int currentPage = 1, string searchTerm = "")
        {
            var result = await business.GetCustomersAsync();

            //Paging
            CurrentPage = currentPage;
            int pageSize = 5;

            if (result != null && result.Status > 0 && result.Data != null)
            {
                Customer = result.Data as List<Customer>;
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                result = await business.SearchByCustomerName(searchTerm);
                Customer = result.Data as List<Customer>;

                return Page();
            }


            int totalOrderDetail = Customer.Count();
            TotalPages = (int)Math.Ceiling((double)totalOrderDetail / pageSize);

            Customer = Customer.Skip((currentPage - 1) * pageSize).Take(pageSize);

            return Page();
        }
    }
}
