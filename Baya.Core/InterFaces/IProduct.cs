using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Baya.DataLayer.Entities;
using Baya.Core.ViewModels;


namespace Baya.Core.InterFaces
{
   public interface IProduct :IDisposable
    {
        Task<List<Product>> GetMostSellProducts();
        Task<List<Product>> GetNewestProducts();
        Task<List<Product>> GetOffProducts();
        Task<Product> GetProductById(Guid id);

        Task<Product> ShowSingleProductById(Guid id);


        #region Search
        Task<List<Product>> SearchProductByText(string inputtxt);
        Task<List<Category>> SearchProductByCategory(string inputtxt);


        Task<List<Product>> WithoutPage_SearchProduct(string searchTerm, string sort); //نمایش محصولات بدون صفحه بندی
        Task<List<Product>> SearchProduct(string searchTerm, string sort, int skip,int countproduct);//نمایش محصولات با صفحه بندی
        int CountProductSearch(string searchTerm, string sort);



        


        #endregion


    }
}
