using Microsoft.EntityFrameworkCore;
using MomAndChildren.Common;
using MomAndChildren.Data;
using MomAndChildren.Data.DAO;
using MomAndChildren.Data.Models;


namespace MomAndChildren.Business
{
    public interface IPaymentHistoryBusiness
    {
        Task<IMomAndChildrenResult> GetPaymentHistoryList();
        Task<IMomAndChildrenResult> GetPaymentHistoryListByCustomerIdAsync(int id);
        Task<IMomAndChildrenResult> GetPaymentHistoryByIdAsync(int id);
        Task<IMomAndChildrenResult> CreatePaymentHistory(Payment paymentHistory);
        Task<IMomAndChildrenResult> UpdatePayment(Payment paymentHistory);
        Task<IMomAndChildrenResult> RemovePayment(int id);
    }

    public class PaymentHistoryBusiness : IPaymentHistoryBusiness
    {
        /*private readonly Net1710_221_3_MomAndChildrenContext _context;
        private readonly PaymentHistoryDAO _DAO;*/
        private readonly UnitOfWork _unitOfWork;

        public PaymentHistoryBusiness()
        {
            _unitOfWork ??= new UnitOfWork();
        }


        public async Task<IMomAndChildrenResult> CreatePaymentHistory(Payment payment)
        {
            try
            {
                _unitOfWork.PaymentHistoryRepository.PrepareCreate(payment);
                int result = await _unitOfWork.PaymentHistoryRepository.SaveAsync();
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
                return new MomAndChildrenResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }

        public async Task<IMomAndChildrenResult> GetPaymentHistoryByIdAsync(int id)
        {
            try
            {
                var payment = await _unitOfWork.PaymentHistoryRepository.GetByIdAsync(id);
                if (payment == null) return new MomAndChildrenResult(Const.WARNING_NO_DATA_CODE, "Payment not found with id: " + id);
                else return new MomAndChildrenResult(Const.SUCCESS_READ_CODE, "Get Payment by id: " + id + " successfully", payment);
            }
            catch (Exception ex)
            {
                return new MomAndChildrenResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }

        public async Task<IMomAndChildrenResult> GetPaymentHistoryListByCustomerIdAsync(int id)
        {
            try
            {
                var paymentList = await _unitOfWork.PaymentHistoryRepository.GetPaymentHistoryListByCustomerId(id);
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

        public async Task<IMomAndChildrenResult> GetPaymentHistoryList()
        {
            try
            {
                var list = await _unitOfWork.PaymentHistoryRepository.GetAllAsync();
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
                _unitOfWork.PaymentHistoryRepository.PrepareUpdate(paymentHistory);
                var result = await _unitOfWork.PaymentHistoryRepository.SaveAsync();
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
                return new MomAndChildrenResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }

        public async Task<IMomAndChildrenResult> RemovePayment(int id)
        {
            try
            {
                var removedItem = _unitOfWork.PaymentHistoryRepository.GetById(id);
                _unitOfWork.PaymentHistoryRepository.PrepareRemove(removedItem);
                var result = await _unitOfWork.PaymentHistoryRepository.SaveAsync();
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
    }
}
