using Microsoft.EntityFrameworkCore;
using MomAndChildren.Common;
using MomAndChildren.Data;
using MomAndChildren.Data.DAO;
using MomAndChildren.Data.Models;
using MomAndChildren.Data.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace MomAndChildren.Business
{
    public interface IOrderBusiness
    {
        //Task<IMomAndChildrenResult> CreateOrder(int customerId, List<ProductCart> productCarts);
        Task<IMomAndChildrenResult> CreateOrder(Order order);
        Task<IMomAndChildrenResult> UpdateOrder(Order order);
        Task<IMomAndChildrenResult> DeleteOrder(int orderId);

        Task<IMomAndChildrenResult> GetOrderByIdAsync(int orderId);

        Task<IMomAndChildrenResult> GetAllOrdersWherePaymentIsNotNullAsync();
        Task<IMomAndChildrenResult> GetOrdersAsync();

        Task<IMomAndChildrenResult> SearchByCustomerId(int? customerId);
        Task<IMomAndChildrenResult> SearchByCustomerName(string? searchTerm);

    }

    public class OrderBusiness : IOrderBusiness
    {

        //private readonly OrderDAO _DAO;

        private readonly UnitOfWork _unitOfWork;
        public OrderBusiness()
        {
            //_DAO = new OrderDAO();
            _unitOfWork ??= new UnitOfWork();
        }

        public async Task<IMomAndChildrenResult> GetOrdersAsync()
        {
            try
            {
                var categories = await _unitOfWork.OrderRepository.GetAllOrderAsync();

                if (categories == null)
                {
                    return new MomAndChildrenResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
                }
                else
                {
                    return new MomAndChildrenResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, categories);
                }
            }
            catch (Exception ex)
            {
                return new MomAndChildrenResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        //public async Task<IMomAndChildrenResult> CreateOrder(int customerId, List<ProductCart> productCarts)
        //{
        //    try
        //    {
        //        double totalPrice = 0;
        //        int totalQuantity = 0;

        //        foreach(var productCart in productCarts)
        //        {
        //            totalPrice += productCart.UnitPrice * productCart.QuantityCart;
        //            totalQuantity += productCart.QuantityCart;
        //        }

        //        var order = new Order()
        //        {
        //            CustomerId = customerId,
        //            TotalPrice = totalPrice,
        //            OrderDate = DateTime.Now,
        //            TotalQuantity = totalQuantity,
        //            OrderDetails = productCarts.Select(p => new OrderDetail()
        //            {
        //                ProductId = p.Product.ProductId,
        //                Quantity = p.QuantityCart,
        //                UnitPrice = p.UnitPrice
        //            }).ToList()
        //        };

        //        //await _context.Orders.AddAsync(order);
        //        //await _context.SaveChangesAsync();

        //        //_DAO.CreateAsync(order);
        //        int result =await _unitOfWork.OrderRepository.CreateAsync(order);

        //        if (result > 0 )
        //        {
        //            return new MomAndChildrenResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG);
        //        }
        //        else
        //        {
        //            return new MomAndChildrenResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
        //        }


        //    }catch(Exception ex)
        //    {
        //        return new MomAndChildrenResult(-1, ex.Message);
        //    }
        //}



        public async Task<IMomAndChildrenResult> CreateOrder(Order order)
        {
            try
            {
                //var customers = await _unitOfWork.CustomerRepository.GetAllAsync();
                   
                //foreach (var customer in customers)
                //    {
                //        if (!(order.CustomerId == customer.CustomerId))
                //        {
                //            return new MomAndChildrenResult(Const.FAIL_CREATE_CODE, "Customer does not exist");
                //        }
                //    }
                
                

                int result = await _unitOfWork.OrderRepository.CreateAsync(order);

                if (result > 0)
                {
                    return new MomAndChildrenResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG);
                }
                else
                {
                    return new MomAndChildrenResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
                }
            }
            catch (Exception ex)
            {
                return new MomAndChildrenResult(Const.ERROR_EXCEPTION, ex.Message);
            }

        }


        public async Task<IMomAndChildrenResult> GetOrderByIdAsync(int orderId)
        {
            try
            {
                var order = await _unitOfWork.OrderRepository.GetByIdAsync(orderId);
                if (order == null)
                {
                    return new MomAndChildrenResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
                }
                else
                {
                    return new MomAndChildrenResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, order);
                }
            }
            catch (Exception ex)
            {
                return new MomAndChildrenResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IMomAndChildrenResult> UpdateOrder(Order order)
        {
            try
            {
                //var customers = await _unitOfWork.CustomerRepository.GetAllAsync();

                //foreach (var customer in customers)
                //{
                //    if (!(order.CustomerId == customer.CustomerId))
                //    {
                //        return new MomAndChildrenResult(Const.FAIL_CREATE_CODE, "Customer does not exist");
                //    }
                //}
                var newOrder = await _unitOfWork.OrderRepository.GetByIdAsync(order.OrderId);
                if (newOrder != null)
                {
                    newOrder.OrderDate = order.OrderDate;
                    newOrder.TotalPrice = order.TotalPrice;
                    newOrder.TotalQuantity = order.TotalQuantity;
                    newOrder.ShippingDate = order.ShippingDate;
                    newOrder.ShippingMethod = order.ShippingMethod;
                    newOrder.OrderNote = order.OrderNote;
                    newOrder.CreateAt = order.CreateAt;
                    newOrder.UpdateAt  = order.UpdateAt;
                    newOrder.CustomerId = order.CustomerId;
                    //newCategory.Status = category.Status;
                    int result = await _unitOfWork.OrderRepository.UpdateAsync(newOrder);
                    if (result > 0)
                    {
                        return new MomAndChildrenResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG);
                    }
                    else
                    {
                        return new MomAndChildrenResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
                    }
                }
                else
                {
                    return new MomAndChildrenResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG);
                }
            }
            catch (Exception ex)
            {
                return new MomAndChildrenResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IMomAndChildrenResult> DeleteOrder(int orderId)
        {
            try
            {
                var order = await _unitOfWork.OrderRepository.GetByIdAsync(orderId);
                if (order != null)
                {
                    ////xem bang product co category muon xóa ko, có -> thì ko cho xóa; ko có -> chuyển status
                    //var orders = await _unitOfWork.OrderRepository.GetAllAsync();
                    //foreach (var order in orders)
                    //{
                    //    if (orderId == order.Ordde.CategoryId)
                    //    {
                    //        return new MomAndChildrenResult(Const.ERROR_EXCEPTION, "Category is using");
                    //    }
                    //}

                    bool result = await _unitOfWork.OrderRepository.RemoveAsync(order);
                    if (result)
                    {
                        return new MomAndChildrenResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG);
                    }
                    else
                    {
                        return new MomAndChildrenResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG);
                    }
                }
                else
                {
                    return new MomAndChildrenResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG);
                }
            }
            catch (Exception ex)
            {
                return new MomAndChildrenResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IMomAndChildrenResult> SearchByCustomerId(int? customerId)
        {
            try
            {
                var orders = await _unitOfWork.OrderRepository.GetAllAsync();
                var result = orders.Where(c => c.CustomerId == customerId).ToList();
                return new MomAndChildrenResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result);
            }
            catch (Exception ex)
            {
                return new MomAndChildrenResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IMomAndChildrenResult> GetAllOrdersWherePaymentIsNotNullAsync()
        {
            try
            {
                var orders = await _unitOfWork.OrderRepository.GetOrdersWherePaymentIsNotNullAsync();

                return new MomAndChildrenResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, orders);
            }
            catch (Exception ex)
            {
                return new MomAndChildrenResult(Const.FAIL_READ_CODE, Const.WARNING_NO_DATA__MSG);
            }
        }

        public async Task<IMomAndChildrenResult> SearchByCustomerName(string? searchTerm)
        {
            try
            {
                var orders = await _unitOfWork.OrderRepository.GetAllAsync();
                var result = orders.Where(c => c.Customer.CustomerName.ToLower().Contains(searchTerm.ToLower()) || c.ShippingMethod.ToLower().Contains(searchTerm.ToLower()) || c.OrderNote.ToLower().Contains(searchTerm.ToLower())).ToList();
                return new MomAndChildrenResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result);
            }
            catch (Exception ex)
            {
                return new MomAndChildrenResult(Const.ERROR_EXCEPTION, ex.Message);
            }

        }
    }
}