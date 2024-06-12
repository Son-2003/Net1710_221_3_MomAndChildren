using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MomAndChildren.Business;
using MomAndChildren.Data.Models.DTO;
using MomAndChildren.Data.Models;
using Microsoft.AspNetCore.Cors.Infrastructure;
using MomAndChildren.Data.Repositories;
using MomAndChildren.Data;
using System.ComponentModel.DataAnnotations;

namespace MomAndChildren.RazorWebApp.Pages
{
    public class CartModel : PageModel
    {
        private readonly ICartBusiness _cartService;
        private readonly IOrderDetailBusiness _orderDetail = new OrderDetailBusiness();
        private readonly IOrderBusiness _order = new OrderBusiness();
        private readonly IProductBusiness _product = new ProductBusiness();
        public CartModel(ICartBusiness cartService)
        {
            _cartService = cartService;
        }
        public string Message { get; set; } = default;

        public List<CartItem> CartItems { get; set; }
        public List<Product> Products { get; set; }
        public List<Order> OrderIds { get; set; }
        [BindProperty]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int ProductId { get; set; }

        [BindProperty]
        public int OrderId { get; set; }

        [BindProperty]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        public void OnGet()
        {
            CartItems = _cartService.GetCartItems();
            Products = this.GetProducts();
            OrderIds = this.GetOrderIds();
        }

        private List<Product> GetProducts()
        {
            var productResult = _product.GetProductsAsync();

            if (productResult.Status > 0 && productResult.Result.Data != null)
            {
                var products = (List<Product>)productResult.Result.Data;
                return products;
            }
            return new List<Product>();
        }

        private List<Order> GetOrderIds()
        {
            var orderIdResult = _order.GetOrdersAsync();

            if (orderIdResult.Status > 0 && orderIdResult.Result.Data != null)
            {
                var orderIds = (List<Order>)orderIdResult.Result.Data;
                return orderIds;
            }
            return new List<Order>();
        }

        public IActionResult OnPostAddToCart()
        {
            _cartService.AddProductToCart(ProductId, Quantity);
            return RedirectToPage();
            
        }

        public IActionResult OnPostCreateOrder()
        {
            var cartItems = _cartService.GetCartItems();
            if (cartItems.Count > 0)
            {
                var orderDetailResult = _orderDetail.CreateOrderDetail(OrderId, cartItems);
                if (orderDetailResult != null)
                {
                    this.Message = orderDetailResult.Result.Message;
                }
                else
                {
                    this.Message = "Error system";
                }
                _cartService.ClearCart();
                return RedirectToPage("OrderDetail");
            }
            return Page();
        }

    }
}
