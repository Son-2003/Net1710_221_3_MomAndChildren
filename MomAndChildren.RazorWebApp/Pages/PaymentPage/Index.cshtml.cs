using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MomAndChildren.Business;
using MomAndChildren.Data.Models;
using System.Collections.Generic;

namespace MomAndChildren.RazorWebApp.Pages.PaymentPage
{
    public class IndexModel : PageModel
    {
        private readonly IPaymentBusiness paymentBusiness;

        public IndexModel()
        {
            paymentBusiness ??= new PaymentBusiness();
        }

        public IEnumerable<Payment> Payment { get; set; } = default!;

        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }


        public async Task<IActionResult> OnGetAsync(int currentPage = 1, string searchTerm = "")
        {
            //var result = await PaymentBusiness.GetPaymentList();
            //if (result != null && result.Status > 0 && result.Data != null)
            //{
            //    Payment = (List<Payment>)result.Data;
            //}
            var result = await paymentBusiness.GetPaymentList();

            //Paging
            CurrentPage = currentPage;
            int pageSize = 5;

            //Search
            if (result != null && result.Status > 0 && result.Data != null)
            {
                Payment = (List<Payment>)result.Data;
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                result = await paymentBusiness.SearchByKeyword(searchTerm);
                Payment = (List<Payment>)result.Data;   

                return Page();
            }

            int totalPayment = Payment.Count();
            TotalPages = (int)Math.Ceiling((double)totalPayment / pageSize);

            Payment = Payment.Skip((currentPage - 1) * pageSize).Take(pageSize);

            return Page();
        }
    }
}
