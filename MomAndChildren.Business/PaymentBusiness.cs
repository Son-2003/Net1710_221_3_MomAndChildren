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
                payment.CreateAt = DateTime.Now;
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
            catch (Exception ex)
            {
                return new MomAndChildrenResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }

        public async Task<IMomAndChildrenResult> GetPaymentByIdAsync(int id)
        {
            try
            {
                var payment = await _unitOfWork.PaymentRepository.GetByIdAsync(id);
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
                var list = await _unitOfWork.PaymentRepository.GetAllAsync();
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
                _unitOfWork.PaymentRepository.PrepareUpdate(paymentHistory);
                var result = await _unitOfWork.PaymentRepository.SaveAsync();
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
    }
}
