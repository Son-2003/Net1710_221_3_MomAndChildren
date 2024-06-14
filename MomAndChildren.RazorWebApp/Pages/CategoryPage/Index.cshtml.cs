using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MomAndChildren.Business;
using MomAndChildren.Data.Models;

namespace MomAndChildren.RazorWebApp.Pages.CategoryPage
{
    public class IndexModel : PageModel
    {
        private readonly ICategoryBusiness business;

        public IndexModel()
        {
            business ??= new CategoryBusiness();
        }

        public IList<Category> Category { get;set; } = default!;

        public string? Name { get; set; }

        public async Task OnGetAsync(string searchTerm = "")
        {
            var result = await business.GetCategoriesAsync();
            if (result != null && result.Status > 0 && result.Data != null)
            {
                Category = result.Data as List<Category>;
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                result = await business.SearchByName(searchTerm);
                Category = result.Data as List<Category>;
            }
        }
    }
}
