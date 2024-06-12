using Microsoft.AspNetCore.Http;
using MomAndChildren.Data;
using MomAndChildren.Data.Models;
using MomAndChildren.Data.Models.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomAndChildren.Business
{
        public interface ICartBusiness
        {
            void AddProductToCart(int productId, int quantity);
            List<CartItem> GetCartItems();
            void ClearCart();
        }
        public class CartBusiness : ICartBusiness
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly UnitOfWork _unitOfWork;
            public CartBusiness(IHttpContextAccessor httpContextAccessor)
            {
                _httpContextAccessor = httpContextAccessor;
                _unitOfWork ??= new UnitOfWork();
            }

            private ISession Session => _httpContextAccessor.HttpContext.Session;

            public void AddProductToCart(int productId, int quantity)
            {
                var cart = GetCartItems();
                var cartItem = cart.FirstOrDefault(ci => ci.ProductId == productId);

                if (cartItem == null)
                {
                    var product = _unitOfWork.ProductRepository.GetById(productId); // Implement this method to get product details
                    cart.Add(new CartItem { ProductId = productId, Product = product, Quantity = quantity });
                }
                else
                {
                    cartItem.Quantity += quantity;
                }

                SaveCart(cart);
            }

            public List<CartItem> GetCartItems()
            {
                var cartJson = Session.GetString("Cart");
                if (string.IsNullOrEmpty(cartJson))
                {
                    return new List<CartItem>();
                }
                return JsonConvert.DeserializeObject<List<CartItem>>(cartJson);
            }

            public void ClearCart()
            {
                Session.Clear();
            }

            private void SaveCart(List<CartItem> cart)
            {
                var cartJson = JsonConvert.SerializeObject(cart);
                Session.SetString("Cart", cartJson);
            }
        }
}
