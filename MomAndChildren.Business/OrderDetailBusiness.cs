using MomAndChildren.Common;
using MomAndChildren.Data;
using MomAndChildren.Data.Models;
using MomAndChildren.Data.Models.DTO;
using MomAndChildren.Data.Repositories;


namespace MomAndChildren.Business
{
    public interface IOrderDetailBusiness
    {
        Task<IMomAndChildrenResult> GetOrderDetailsAsync();
        Task<IMomAndChildrenResult> GetOrderDetailByIdAsync(int orderDetailId);
        Task<IMomAndChildrenResult> CreateOrderDetail(int orderId, List<CartItem> products);
        Task<IMomAndChildrenResult> UpdateOrderDetailQuantity(int detailId, int quantity);
        Task<IMomAndChildrenResult> DeleteOrderDetail(int detailId);
    }

    public class OrderDetailBusiness : IOrderDetailBusiness
    {
        //private readonly Net1710_221_3_MomAndChildrenContext _context;
        //private readonly OrderDetailDAO _DAO;
        private readonly UnitOfWork _unitOfWork;

        public OrderDetailBusiness()
        {
            _unitOfWork ??= new UnitOfWork();
        }

        public async Task<IMomAndChildrenResult> CreateOrderDetail(int orderId, List<CartItem> products)
        {
            //create new order details
            foreach (var item in products)
            {
                double price = item.Product.Price;
                int quantityCart = item.Quantity;

                var orderDetail = new OrderDetail
                {
                    OrderId = orderId,
                    UnitPrice = price,
                    TotalPrice = price * quantityCart,
                    ProductId = item.Product.ProductId,
                    Quantity = quantityCart,
                };
                await _unitOfWork.OrderDetailRepository.CreateAsync(orderDetail);
            }
            return new MomAndChildrenResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG);
        }

        public async Task<IMomAndChildrenResult> GetOrderDetailByIdAsync(int orderDetailId)
        {
            OrderDetail? orderDetail = await _unitOfWork.OrderDetailRepository.GetByIdAsync(orderDetailId);
            if (orderDetail == null)
            {
                return new MomAndChildrenResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
            }
            else
            {
                return new MomAndChildrenResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, orderDetail);
            }
        }

        public async Task<IMomAndChildrenResult> GetOrderDetailsAsync()
        {
            try
            {
                var orderDetails = await _unitOfWork.OrderDetailRepository.GetAllAsyncOrderDetails();
                if (orderDetails == null)
                {
                    return new MomAndChildrenResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
                }
                else
                {
                    return new MomAndChildrenResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, orderDetails);
                }
            }
            catch (Exception ex)
            {
                return new MomAndChildrenResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IMomAndChildrenResult> UpdateOrderDetailQuantity(int detailId, int quantity)
        {
            var orderDetail = await _unitOfWork.OrderDetailRepository.GetByIdAsync(detailId);
            if (orderDetail != null)
            {
                orderDetail.Quantity = quantity;
                orderDetail.TotalPrice = quantity * orderDetail.UnitPrice;
                await _unitOfWork.OrderDetailRepository.UpdateAsync(orderDetail);
                return new MomAndChildrenResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG);
            }
            return new MomAndChildrenResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
        }

        public async Task<IMomAndChildrenResult> DeleteOrderDetail(int detailId)
        {
            var orderDetail = await _unitOfWork.OrderDetailRepository.GetByIdAsync(detailId);
            if (orderDetail != null)
            {
                await _unitOfWork.OrderDetailRepository.RemoveAsync(orderDetail);
                return new MomAndChildrenResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG);
            }
            return new MomAndChildrenResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG);

        }
    }
}
