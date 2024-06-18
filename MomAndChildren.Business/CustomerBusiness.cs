using MomAndChildren.Common;
using MomAndChildren.Data.Models;
using MomAndChildren.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomAndChildren.Business
{
    public interface ICustomerBusiness
    {
        //Task<IMomAndChildrenResult> CreateOrder(int customerId, List<ProductCart> productCarts);
        Task<IMomAndChildrenResult> CreateCustomer(Customer customer);
        Task<IMomAndChildrenResult> UpdateCustomer(Customer customer);
        Task<IMomAndChildrenResult> DeleteCustomer(int customerId);

        Task<IMomAndChildrenResult> GetCustomerByIdAsync(int customerId);
        Task<IMomAndChildrenResult> GetCustomersAsync();

        Task<IMomAndChildrenResult> SearchByCustomerName(string? searchTerm);
    }

    public class CustomerBusiness : ICustomerBusiness
    {

        //private readonly OrderDAO _DAO;

        private readonly UnitOfWork _unitOfWork;
        public CustomerBusiness()
        {
            //_DAO = new OrderDAO();
            _unitOfWork ??= new UnitOfWork();
        }

        public async Task<IMomAndChildrenResult> GetCustomersAsync()
        {
            try
            {
                var customers = await _unitOfWork.CustomerRepository.GetAllAsync();

                if (customers == null)
                {
                    return new MomAndChildrenResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
                }
                else
                {
                    return new MomAndChildrenResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, customers);
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



        public async Task<IMomAndChildrenResult> CreateCustomer(Customer customer)
        {
            try
            {
                //check trung ten
                //var categories = await _context.Categories.OrderByDescending(c => c.CategoryName).ToListAsync();
                //var categories = await _CategoryDAO.GetAllAsync();
                //var orders = await _unitOfWork.OrderRepository.GetAllAsync();
                //foreach (var item in orders)
                //{
                //    if (order.OrderId.Equals(item.OrderId))
                //    {
                //        return new MomAndChildrenResult(Const.ERROR_EXCEPTION, "Name is duplicated.");
                //    }
                //}

                int result = await _unitOfWork.CustomerRepository.CreateAsync(customer);

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


        public async Task<IMomAndChildrenResult> GetCustomerByIdAsync(int customerId)
        {
            try
            {
                var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(customerId);
                if (customer == null)
                {
                    return new MomAndChildrenResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
                }
                else
                {
                    return new MomAndChildrenResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, customer);
                }
            }
            catch (Exception ex)
            {
                return new MomAndChildrenResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IMomAndChildrenResult> UpdateCustomer(Customer customer)
        {
            try
            {
                var newCustomer = await _unitOfWork.CustomerRepository.GetByIdAsync(customer.CustomerId);
                if (newCustomer != null)
                {
                    newCustomer.CustomerName = customer.CustomerName;
                    newCustomer.Address = customer.Address;
                    newCustomer.PhoneNumber = customer.PhoneNumber;
                    newCustomer.Dob = customer.Dob;
                    newCustomer.Gender = customer.Gender;
                    newCustomer.Email = customer.Email;
                    newCustomer.Bio = customer.Bio;
                    newCustomer.CreateAt = customer.CreateAt;
                    newCustomer.UpdateAt = customer.UpdateAt;
                    //newCategory.Status = category.Status;
                    int result = await _unitOfWork.CustomerRepository.UpdateAsync(newCustomer);
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

        public async Task<IMomAndChildrenResult> DeleteCustomer(int customerId)
        {
            try
            {
                var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(customerId);
                if (customer != null)
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

                    bool result = await _unitOfWork.CustomerRepository.RemoveAsync(customer);
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


        public async Task<IMomAndChildrenResult> SearchByCustomerName(string? searchTerm)
        {
            try
            {
                var customers = await _unitOfWork.CustomerRepository.GetAllAsync();
                var result = customers.Where(c => c.CustomerName.ToLower().Contains(searchTerm.ToLower()) || c.Address.ToLower().Contains(searchTerm.ToLower()) || c.Email.ToLower().Contains(searchTerm.ToLower())).ToList();
                return new MomAndChildrenResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result);
            }
            catch (Exception ex)
            {
                return new MomAndChildrenResult(Const.ERROR_EXCEPTION, ex.Message);
            }

        }
    }
}
