using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MomAndChildren.Business;
using MomAndChildren.Data.Models;

namespace MomAndChildren.RazorWebApp.Pages
{
    public class ProductModel : PageModel
    {
        private readonly IProductBusiness _productBusiness = new ProductBusiness();
        public string Message { get; set; } = default;
        [BindProperty]
        public Product Product { get; set; } = default;
        public List<Product> Products { get; set; } = new List<Product>();

        public void OnGet()
        {
            Products = this.GetProducts();
        }

        public IActionResult OnPost(int productId, Product Product)
        {
            if (Product != null)
            {
                _productBusiness.UpdateProduct(productId, Product);
            }
            return Redirect("Product");
        }

        private List<Product> GetProducts()
        {
            var productsResult = _productBusiness.GetProductsAsync();

            if (productsResult.Status > 0 && productsResult.Result.Data != null)
            {
                var Products = productsResult.Result.Data;
                return (List<Product>)Products;
            }
            return new List<Product>();
        }

        public IActionResult OnPostDelete(int productId)
        {
            var ProductResult = _productBusiness.GetProductByIdAsync(productId);

            if (ProductResult != null)
            {
                _productBusiness.DeleteProduct(productId);
            }

            return Redirect("Product");
        }
    }
}
