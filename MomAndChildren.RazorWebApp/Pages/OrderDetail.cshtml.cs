using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MomAndChildren.Business;
using MomAndChildren.Data.Models;

namespace MomAndChildren.RazorWebApp.Pages
{
    public class OrderDetailModel : PageModel
    {
        private readonly IOrderDetailBusiness _orderDetailBusiness = new OrderDetailBusiness();
        public string Message { get; set; } = default;
        [BindProperty]
        public OrderDetail OrderDetail { get; set; } = default;
        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        public void OnGet()
        {
            OrderDetails = this.GetOrderDetails();
        }

        public IActionResult OnPost(Dictionary<int, int> orderDetails)
        {
            if (orderDetails != null)
            {
                foreach (var detail in orderDetails)
                {
                    _orderDetailBusiness.UpdateOrderDetailQuantity(detail.Key, detail.Value);
                }
            }
            return Redirect("OrderDetail");
        }

        private List<OrderDetail> GetOrderDetails()
        {
            var orderDetailResult = _orderDetailBusiness.GetOrderDetailsAsync();

            if (orderDetailResult.Status > 0 && orderDetailResult.Result.Data != null)
            {
                var orderDetails = orderDetailResult.Result.Data;
                return (List<OrderDetail>)orderDetails;
            }
            return new List<OrderDetail>();
        }

        public IActionResult OnPostDelete(int orderDetailId)
        {
            var orderDetailResult = _orderDetailBusiness.GetOrderDetailByIdAsync(orderDetailId);

            if (orderDetailResult != null)
            {
                _orderDetailBusiness.DeleteOrderDetail(orderDetailId);
            }

            return Redirect("OrderDetail");
        }


    }
}
