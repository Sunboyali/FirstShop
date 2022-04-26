using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Baya.Core.ViewModels;
using Baya.DataLayer.Entities;

namespace Baya.Core.InterFaces
{
   public interface IScope : IDisposable
    {
        int CountCartUser(Guid Iduser);
        //long SumPriceUserCart(Guid Iduser);
       


        void AddCard(Guid idProduct, Guid idUser);

        void UpdateSumPrice(Guid orderid);

        Task<OrderDetail> GetOrderDetailById(Guid id);
        void DeleteBasketProduct(long totalprice, Guid id);

        Task<List<ShowCardViewModel>> ShowBasketUser(Guid idUser);


        //Task<UserDetail> GetInformationUserForPayment(Guid id);

        Task<List<City>> GetCities();

        Guid GetStateIdByName(string name);
        Guid GetCityIdByName(string name);

        bool UserHaveAddress(Guid id);

        Task<Array> GetUserInfo(Guid id);


        void AddAddressUser(Guid id, AddAddressInfoViewModel model);



        #region Favorites
        int CountFavoritesUser(Guid Iduser);
        Task<List<UserFavoriteViewModel>> GetFavoritesUser(Guid Iduser);
        void DeleteFavorite(Guid id);
        bool IsSelectedFavorite(Guid productid, Guid userid);
        bool AddFavorite(Guid idProduct, Guid idUser);
        #endregion


        #region Categories
        Task<List<Category>> ShowCategory();
        bool HaveAnySubCategory(Guid id);

        #endregion

        #region Sliders
        Task<List<SliderImage>> GetSlidersImage();
        #endregion

        #region Banner
        Task<List<Banner>> GetBannersImage();
        #endregion


        #region Theme
        Theme GetSettingTheme();
        #endregion

    }
}
