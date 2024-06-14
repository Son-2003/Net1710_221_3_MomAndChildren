using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MomAndChildren.Business;
using MomAndChildren.Data.Models;

namespace MomAndChildren.RazorWebApp.Pages.BrandPage
{
    public class IndexModel : PageModel
    {
        private readonly IBrandBusiness business;

        public IndexModel()
        {
            business ??= new BrandBusiness();
        }

        public IList<Brand> Brand { get; set; } = default!;

        public string? Name { get; set; }

        public async Task OnGetAsync(string searchTerm = "")
        {
            var result = await business.GetBrandsAsync();
            if (result != null && result.Status > 0 && result.Data != null)
            {
                Brand = result.Data as List<Brand>;
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                result = await business.SearchByName(searchTerm);
                Brand = result.Data as List<Brand>;
            }
        }
    }
}
