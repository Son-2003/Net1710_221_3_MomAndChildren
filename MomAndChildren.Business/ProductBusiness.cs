using MomAndChildren.Common;
using MomAndChildren.Data;
using MomAndChildren.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomAndChildren.Business
{

    public interface IProductBusiness
    {
        Task<IMomAndChildrenResult> GetProductsAsync();
        Task<IMomAndChildrenResult> GetProductByIdAsync(int id);
        Task<IMomAndChildrenResult> CreateProduct(Product product);
        Task<IMomAndChildrenResult> UpdateProduct(Product product);
        Task<IMomAndChildrenResult> DeleteProduct(int productId);
        bool ProductExists(int id);
    }

    public class ProductBusiness : IProductBusiness
    {
        private readonly UnitOfWork _unitOfWork;

        public ProductBusiness()
        {
            _unitOfWork ??= new UnitOfWork();
        }

        public async Task<IMomAndChildrenResult> CreateProduct(Product product)
        {
            if (_unitOfWork.ProductRepository.GetAll().Any(p => p.ProductId == product.ProductId))
            {
                return new MomAndChildrenResult(-1, "Product id is duplicate");
            }
            await _unitOfWork.ProductRepository.CreateAsync(product);
            await _unitOfWork.ProductRepository.SaveAsync();
            return new MomAndChildrenResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG);
        }

        public async Task<IMomAndChildrenResult> DeleteProduct(int productId)
        {
            Product product = _unitOfWork.ProductRepository.GetById(productId);
            if (product == null)
            {
                return new MomAndChildrenResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG);
            }
            else
            {
                await _unitOfWork.ProductRepository.RemoveAsync(product);
                await _unitOfWork.ProductRepository.SaveAsync();
                return new MomAndChildrenResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG);
            }
        }

        public async Task<IMomAndChildrenResult> GetProductByIdAsync(int productId)
        {
            Product product = await _unitOfWork.ProductRepository.GetByIdAsync(productId);
            if (product == null)
            {
                return new MomAndChildrenResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG);
            }
            else
            {
                return new MomAndChildrenResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, product);
            }
        }

        public async Task<IMomAndChildrenResult> GetProductsAsync()
        {
            List<Product> products = await _unitOfWork.ProductRepository.GetAllAsync();
            return new MomAndChildrenResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, products);
        }

        public bool ProductExists(int id)
        {
            return _unitOfWork.ProductRepository.GetAll().Any(e => e.ProductId == id);
        }

        public async Task<IMomAndChildrenResult> UpdateProduct(Product product)
        {
            await _unitOfWork.ProductRepository.UpdateAsync(product);
            await _unitOfWork.ProductRepository.SaveAsync();
            return new MomAndChildrenResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG);
        }
    }
}
