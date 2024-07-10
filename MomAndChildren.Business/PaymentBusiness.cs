using Microsoft.EntityFrameworkCore;
using MomAndChildren.Common;
using MomAndChildren.Data;
using MomAndChildren.Data.DAO;
using MomAndChildren.Data.Models;


namespace MomAndChildren.Business
{
    public interface IPaymentBusiness
    {
        Task<IMomAndChildrenResult> GetPaymentList();
        Task<IMomAndChildrenResult> GetPaymentListByCustomerIdAsync(int id);
        Task<IMomAndChildrenResult> GetPaymentByIdAsync(int id);
        Task<IMomAndChildrenResult> CreatePayment(Payment paymentHistory);
        Task<IMomAndChildrenResult> UpdatePayment(Payment paymentHistory);
        Task<IMomAndChildrenResult> RemovePayment(int id);
        Task<IMomAndChildrenResult> SearchByKeyword(string? searchTerm);
    }

    public class PaymentBusiness : IPaymentBusiness
    {
        /*private readonly Net1710_221_3_MomAndChildrenContext _context;
        private readonly PaymentHistoryDAO _DAO;*/
        private readonly UnitOfWork _unitOfWork;

        public PaymentBusiness()
        {
            _unitOfWork ??= new UnitOfWork();
        }


        public async Task<IMomAndChildrenResult> CreatePayment(Payment payment)
        {
            try
            {
                if (CheckOrderIdInUse(payment.OrderId))
                {
                    return new MomAndChildrenResult(-1, "This OrderId is already associated with another payment.");
                }
                else
                {
                    _unitOfWork.PaymentRepository.PrepareCreate(payment);
                    int result = await _unitOfWork.PaymentRepository.SaveAsync();
                    if (result > 0)
                    {
                        return new MomAndChildrenResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG);
                    }
                    else
                    {
                        return new MomAndChildrenResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
                    }
                }
            }
            catch (Exception ex)
            {
                return new MomAndChildrenResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }

        public async Task<IMomAndChildrenResult> GetPaymentByIdAsync(int id)
        {
            try
            {
                var payment = _unitOfWork.PaymentRepository.GetPaymentById(id);
                if (payment == null) return new MomAndChildrenResult(Const.WARNING_NO_DATA_CODE, "Payment not found with id: " + id);
                else return new MomAndChildrenResult(Const.SUCCESS_READ_CODE, "Get Payment by id: " + id + " successfully", payment);
            }
            catch (Exception ex)
            {
                return new MomAndChildrenResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }

        public async Task<IMomAndChildrenResult> GetPaymentListByCustomerIdAsync(int id)
        {
            try
            {
                var paymentList = await _unitOfWork.PaymentRepository.GetPaymentListByCustomerId(id);
                if (paymentList == null)
                {
                    return new MomAndChildrenResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG);
                }
                else
                {
                    return new MomAndChildrenResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, paymentList);
                }
            }
            catch (Exception ex)
            {
                return new MomAndChildrenResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }

        public async Task<IMomAndChildrenResult> GetPaymentList()
        {
            try
            {
                var list = await _unitOfWork.PaymentRepository.GetAllPaymentAsync();
                if (list == null)
                {
                    return new MomAndChildrenResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG);
                }
                else
                {
                    return new MomAndChildrenResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, list);
                }
            }
            catch (Exception ex)
            {
                return new MomAndChildrenResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IMomAndChildrenResult> UpdatePayment(Payment paymentHistory)
        {
            try
            {
                // Retrieve existing payment by payment ID
                var existed = await _unitOfWork.PaymentRepository.GetByIdAsync(paymentHistory.PaymentId);
                if (existed == null)
                {
                    return new MomAndChildrenResult(Const.FAIL_UPDATE_CODE, "Payment not found.");
                }

                // If the order ID of the existing payment does not match the new one
                if (existed.OrderId != paymentHistory.OrderId)
                {
                    // Check if the new order ID is already in use by another payment
                    if (CheckOrderIdInUse(paymentHistory.OrderId))
                    {
                        return new MomAndChildrenResult(-1, "This OrderId is already associated with another payment.");
                    }
                }

                // Prepare the update for the payment
                existed.PaymentMethod = paymentHistory.PaymentMethod;
                existed.Status = paymentHistory.Status;
                existed.PaymentDate = paymentHistory.PaymentDate;
                existed.CreateAt = paymentHistory.CreateAt;
                existed.UpdateAt = paymentHistory.UpdateAt;
                existed.Note = paymentHistory.Note;
                existed.BillingAddress = paymentHistory.BillingAddress;
                existed.Currency = paymentHistory.Currency;
                existed.OrderId = paymentHistory.OrderId; // Ensure this updates the Order reference correctly

                _unitOfWork.PaymentRepository.PrepareUpdate(existed);

                // Save the changes asynchronously
                var result = await _unitOfWork.PaymentRepository.SaveAsync();

                // Check the result and return appropriate response
                if (result > 0)
                {
                    return new MomAndChildrenResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG);
                }
                else
                {
                    return new MomAndChildrenResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions and return an error result
                return new MomAndChildrenResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }


        public async Task<IMomAndChildrenResult> RemovePayment(int id)
        {
            try
            {
                var removedItem = _unitOfWork.PaymentRepository.GetById(id);
                _unitOfWork.PaymentRepository.PrepareRemove(removedItem);
                var result = await _unitOfWork.PaymentRepository.SaveAsync();
                if (result > 0)
                {
                    return new MomAndChildrenResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG);
                }
                else
                {
                    return new MomAndChildrenResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG);
                }
            }
            catch (Exception ex)
            {
                return new MomAndChildrenResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }

        public async Task<IMomAndChildrenResult> SearchByKeyword(string? searchTerm)
        {
            try
            {
                var payment = await _unitOfWork.PaymentRepository.GetAllAsync();
                var result = payment.Where(p => p.PaymentMethod.ToLower().Contains(searchTerm.ToLower())
                    || p.BillingAddress.Contains(searchTerm.ToLower())
                    || p.Note.ToLower().Contains(searchTerm.ToLower())
                    || p.Currency.ToLower().Contains(searchTerm.ToLower())
                    ).ToList();
                return new MomAndChildrenResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result);
            }
            catch (Exception ex)
            {
                return new MomAndChildrenResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        private bool CheckOrderIdInUse(int orderId)
        {
            return _unitOfWork.PaymentRepository.IsOrderInUse(orderId);
        }
    }
}
