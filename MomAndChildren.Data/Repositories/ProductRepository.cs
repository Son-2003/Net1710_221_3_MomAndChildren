using Microsoft.EntityFrameworkCore;
using MomAndChildren.Data.Base;
using MomAndChildren.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomAndChildren.Data.Repositories
{
    public class ProductRepository : GenericRepository<Product>
    {
        public ProductRepository() { }
        public ProductRepository(Net1710_221_3_MomAndChildrenContext context) => _context = context;
        public List<Product> getProductsAsync()
        {
            List<Product> products = _context.Products.Include(x => x.Brand).Include(x=>x.Category).ToList();
            return products;
        }
    }
}
