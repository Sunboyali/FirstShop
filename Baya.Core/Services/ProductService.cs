using System;
using System.Collections.Generic;
using System.Text;
using Baya.Core.InterFaces;
using Baya.DataLayer.Entities;
using Baya.DataLayer.BayaContextDb;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Baya.Core.Classes;
using Baya.Core.ViewModels;

namespace Baya.Core.Services
{
    public class ProductService : IProduct
    {
        private readonly BayaDbContext _context;

        public ProductService(BayaDbContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            _context.Dispose();
        }



        public async Task<List<Product>> GetMostSellProducts()
        {
            return await _context.Products.Include(p => p.ProductDetail).Where(p => p.IsShowProduct == true).OrderByDescending(p=> p.CountsSale).Take(12).ToListAsync();
        }

        public async Task<List<Product>> GetNewestProducts()
        {
            return await _context.Products.Include(p => p.ProductDetail).Where(p => p.IsShowProduct == true).OrderByDescending(p => p.ProductDetail.DateProduct).Take(12).ToListAsync();
        }

        public async Task<List<Product>> GetOffProducts()
        {
            return await _context.Products.Include(p => p.ProductDetail).Where(p => p.IsShowProduct == true && p.Off > 0).OrderByDescending(p => p.ProductDetail.DateProduct).Take(12).ToListAsync();
        }

        public async Task<Product> GetProductById(Guid id)
        {
            return await _context.Products.Include(p => p.ProductDetail).FirstOrDefaultAsync(p => p.Id == id);
        }

       

        public async Task<Product> ShowSingleProductById(Guid id)
        {

            var product = await _context.Products.Include(p => p.ProductDetail).SingleOrDefaultAsync(p => p.Id == id);
            product.ProductDetail.VisitCount += 1;
            _context.SaveChanges();

            return product;
        }

        #region Search

        public async Task<List<Product>> SearchProductByText(string inputtxt)
        {
            return await _context.Products.Include(c => c.Category).Where(p => p.ProductName.Contains(inputtxt)).Take(8).ToListAsync();
        }

        public async Task<List<Category>> SearchProductByCategory(string inputtxt)
        {
            return await _context.Categories.Where(c => c.CategoryName.Contains(inputtxt)).Take(3).ToListAsync();
        }





        public async Task<List<Product>> SearchProduct(string searchTerm, string sort, int skip, int countproduct)
        {
            if (sort == null)
            {
                return await _context.Products.Where(p => p.IsShowProduct == true && p.ProductName.Contains(searchTerm.Trim())).Skip(skip).Take(countproduct).ToListAsync();
            }
            else if (sort == "1")//پرفروش ترین
            {
                return await _context.Products.Where(p => p.IsShowProduct == true && p.ProductName.Contains(searchTerm.Trim())).OrderByDescending(p => p.CountsSale).Skip(skip).Take(countproduct).ToListAsync();
            }
            else if (sort == "2") //پربازدیدترین
            {
                return await _context.Products.Where(p => p.IsShowProduct == true && p.ProductName.Contains(searchTerm.Trim())).OrderByDescending(p => p.ProductDetail.VisitCount).Skip(skip).Take(countproduct).ToListAsync();
            }
            else if (sort == "3")//جدیدترین
            {
                return await _context.Products.Where(p => p.IsShowProduct == true && p.ProductName.Contains(searchTerm.Trim())).OrderByDescending(p => p.ProductDetail.DateProduct).Skip(skip).Take(countproduct).ToListAsync();
            }
            else if (sort == "4")//ارزان ترین
            {
                return await _context.Products.Where(p => p.IsShowProduct == true && p.ProductName.Contains(searchTerm.Trim())).OrderBy(p => p.Price).Skip(skip).Take(countproduct).ToListAsync();
            }
            else//گران ترین
            {
                return await _context.Products.Where(p => p.IsShowProduct == true && p.ProductName.Contains(searchTerm.Trim())).OrderByDescending(p => p.Price).Skip(skip).Take(countproduct).ToListAsync();
            }


        }

        public int CountProductSearch(string searchTerm, string sort)
        {
            if (sort == null)
            {
                return _context.Products.Where(p => p.IsShowProduct == true && p.ProductName.Contains(searchTerm)).Count();
            }
            else if (sort == "1")//پرفروش ترین
            {
                return _context.Products.Where(p => p.IsShowProduct == true && p.ProductName.Contains(searchTerm.Trim())).Count();
            }
            else if (sort == "2") //پربازدیدترین
            {
                return _context.Products.Where(p => p.IsShowProduct == true && p.ProductName.Contains(searchTerm.Trim())).Count();
            }
            else if (sort == "3")//جدیدترین
            {
                return _context.Products.Where(p => p.IsShowProduct == true && p.ProductName.Contains(searchTerm.Trim())).Count();
            }
            else if (sort == "4")//ارزان ترین
            {
                return  _context.Products.Where(p => p.IsShowProduct == true && p.ProductName.Contains(searchTerm.Trim())).Count();
            }
            else//گران ترین
            {
                return _context.Products.Where(p => p.IsShowProduct == true && p.ProductName.Contains(searchTerm.Trim())).Count();
            }

        }

        public async Task<List<Product>> WithoutPage_SearchProduct(string searchTerm, string sort)
        {
            if (sort == null)
            {
                return await _context.Products.Where(p => p.IsShowProduct == true && p.ProductName.Contains(searchTerm.Trim())).ToListAsync();
            }
            else if (sort == "1")//پرفروش ترین
            {
                return await _context.Products.Where(p => p.IsShowProduct == true && p.ProductName.Contains(searchTerm.Trim())).OrderByDescending(p => p.CountsSale).ToListAsync();
            }
            else if (sort == "2") //پربازدیدترین
            {
                return await _context.Products.Where(p => p.IsShowProduct == true && p.ProductName.Contains(searchTerm.Trim())).OrderByDescending(p => p.ProductDetail.VisitCount).ToListAsync();
            }
            else if (sort == "3")//جدیدترین
            {
                return await _context.Products.Where(p => p.IsShowProduct == true && p.ProductName.Contains(searchTerm.Trim())).OrderByDescending(p => p.ProductDetail.DateProduct).ToListAsync();
            }
            else if (sort == "4")//ارزان ترین
            {
                return await _context.Products.Where(p => p.IsShowProduct == true && p.ProductName.Contains(searchTerm.Trim())).OrderBy(p => p.Price).ToListAsync();
            }
            else//گران ترین
            {
                return await _context.Products.Where(p => p.IsShowProduct == true && p.ProductName.Contains(searchTerm.Trim())).OrderByDescending(p => p.Price).ToListAsync();
            }
        }

       
        #endregion
    }
}
