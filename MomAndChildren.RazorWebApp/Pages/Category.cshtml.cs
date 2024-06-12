using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MomAndChildren.Business;
using MomAndChildren.Data.Models;

namespace MomAndChildren.RazorWebApp.Pages
{
    public class CategoryModel : PageModel
    {

        private readonly ICategoryBusiness _categoryBusiness = new CategoryBusiness();
        public string Message { get; set; } = default;
        [BindProperty]
        public Category Category { get; set; } = default;
        public List<Category> Categories { get; set; } = new List<Category>();

        public void OnGet()
        {
            Categories = this.GetCategories();
        }

        public void OnPost()
        {
            this.SaveCategory();
        }

        public void OnDelete()
        {
        }


        private List<Category> GetCategories()
        {
            var categoryResult = _categoryBusiness.GetCategoriesAsync();

            if (categoryResult.Status > 0 && categoryResult.Result.Data != null)
            {
                var categories = (List<Category>)categoryResult.Result.Data;
                return categories;
            }
            return new List<Category>();
        }

        private void SaveCategory()
        {
            var categoryResult = _categoryBusiness.CreateCategory(this.Category);

            if (categoryResult != null)
            {
                this.Message = categoryResult.Result.Message;
            }
            else
            {
                this.Message = "Error system";
            }
        }

    }
}
