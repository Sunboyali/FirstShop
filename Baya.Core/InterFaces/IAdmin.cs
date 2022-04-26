using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Baya.DataLayer.Entities;
using Baya.Core.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Baya.Core.InterFaces
{
   public interface IAdmin :IDisposable
    {
        #region Category
        Task<List<Category>> GetGategories();
        Task<Category> GetCategoryById(Guid id);
        Task<string> GetFullCategory(Guid id);
        bool ExistCategory(Guid id);

        bool ExistNameCategoryForAdd(string catname);
        bool ExistNameCategoryForUpdate(string catname, Guid id);

        void AddOrEditMainCategory(MainCategoryViewModel viewModel);
        void AddOrUpdateSubCategory(SubCategoryViewModel model);

        void AddOrUpdateSecondSubCategory(SubCategoryViewModel model);
    
        void DeleteCategory(Guid id);
       


        #endregion

        #region Brand
        Task<List<Brand>> GetBrands();
        Task<Brand> GetBrandById(Guid id);

        void AddBrand(Brand brand);

        void UpdateBrand(Brand brand);

        bool ExistBrandId(Guid id);

        #endregion

        #region User
        Task<List<User>> GetActiveUsers(int skip, int countuser);
        Task<List<User>> GetActiveUsersBySearch(string mobile,int skip, int countuser);
        Task<List<User>> GetDeActiveUsers(int skip, int countuser);
        Task<List<User>> GetDeActiveUsersBySearch(string mobile, int skip, int countuser);

        Task<List<User>> GetBlockedUsers(int skip, int countuser);
        Task<List<User>> GetBlockedUsersBySearch(string mobile, int skip, int countuser);
        int CountActiveUsers();
        int CountActiveUsersBySearch(string mobile);
        int CountDeActiveUsers();
        int CountDeActiveUsersBySearch(string mobile);
        int CountBlockedUsers();
        int CountBlockedUsersBySearch(string mobile);


        Task<User> GetUserById(Guid id);




        void AddUser(UserViewModel user);

        bool CheckMobileNumber(string mobile);

        void UpdateRoleUser(ChangeRoleViewModel model);




        Guid GetRoleId(string name);
       
        
      
        void BlockUser(Guid id);
       
       

       

       

        #endregion

        #region Shopper
        //void AddShopper(Guid id);
        #endregion

        #region Role
        Task<List<Role>> GetRoles();
        Task<Role> GetRoleById(Guid id);
        void AddAndUpdateRole(RoleViewModel model);
        void DeleteRole(Guid id);

        bool ExistRoleName(RoleViewModel model);
        #endregion


        #region Product
        Task<List<Product>> GetProducts(int skip,int countproduct);   
        Task<Product> GetProductById(Guid id);
        int CountProducts();
        bool ShowProduct(Guid id);
        void AddProduct(ProductViewModel model);
        void UpdateProduct(ProductViewModel model);
        int CountProductsBySearch(string code, string name);
        int CountUnShowProductsBySearch(string code, string name);
        Task<List<Product>> SearchProducts(string code, string name, int skip, int countproduct);
        Task<List<Product>> SearchUnShowProducts(string code, string name, int skip, int countproduct);       
        void RomoveProduct(Guid id);
        #endregion

        #region Amazing Products
        void Amazing(Guid id);
        Task<List<Product>> GetAmazingProducts(int skip, int countproduct);
        int CountAmazingProducts();
        int CountAmazingProductsBySearch(string code, string name);
        Task<List<Product>> SearchAmazingProducts(string code, string name, int skip, int countproduct);
        #endregion


        #region UnShowProduct
        Task<List<Product>> GetUnShowProducts(int skip, int countproduct);
        int CountUnShowProducts();
        #endregion

        #region Gallery
        #region Gallery
        Task<List<ProductGallery>> GetLsitProductGallery(Guid id);
        Task<ProductGallery> GetImageGalleryById(Guid id);

        void UpdateOneImageGallery(UpdateImageViewModel model);
        void UpdateMainImageProduct(UpdateImageViewModel model);

        void AddGalleryListForProduct(ProductGalleryViewModel model);

        #endregion
        #endregion

        #region Settings
        Task<Setting> GetSetting();
        bool UpdateSiteSetting(SiteSettingViewModel viewModel);

        #endregion


        #region Polls
        Task<List<Poll>> GetPolls(int skip, int count);

        Task<Poll> GetPollById(Guid id);
        Task<PollOption> GetPollOptionById(Guid id);

        Task<List<PollOption>> GetQuestionsListByPollId(Guid id);

        void AddAndUpdatePoll(PollViewModel viewModel);

        void UpdatePollOption(Guid Id, string polloptionname);

        int CountPolls();

        void ActivatePoll(Guid id);












        void DeletePoll(Guid id);
       


       
        int SumVotes(Guid id);



        //bool ActivePoolExist();
        #endregion

        #region Slider Mananage
        Task<List<Slider>> GetSliders();
        Task<Slider> GetSliderById(Guid id);
        Task<SliderImage> GetSliderImageById(Guid id);

        Task<List<SliderImage>> GetSliderImages(Guid id);
        Task<List<int>> GetDisplaySlides(Guid slideid);
        void AddAndUpdateSlideImage(SlideImageViewModel model);
        string GetSliderName(Guid id);

        void UpdateNumberSlide(Guid id,int number);









        bool IsNumberSlide(Guid id,int numslide);
        void ActivateSlider(Guid id);
       

        void DeleteSlideImage(Guid id);




        #endregion


        #region Banner
        Task<List<Banner>> GetBanners();

        Task<Banner> GetBannerById(Guid id);

        void UpdateBanner(BannerImageViewModel viewModel);

        #endregion


        #region Factors


        int CountFactor();
        int CountFactorByFromDate(string fromdate);
        int CountFactorByToDate(string todate);
        int CountFactorByTwoDate(string fromdate,string todate);
        int CountFactorWithMobile(string mobile);


        Task<List<Factor>> GetSuccessFactors(string search_mobile,int skip,int countfactor);
        Task<List<Factor>> GetFactorsFromDate(string fromdate,int skip,int countfactor);
        Task<List<Factor>> GetFactorsToDate(string todate,int skip,int countfactor);
        Task<List<Factor>> GetFactorsFromAndToDate(string fromdate, string todate,int skip,int countfactor);

        Task<List<OrderDetail>> GetDetailFactor(Guid id);

        #endregion


        #region Color
        Task<List<Color>> GetColors();
        Task<string> GetFullColorNamesForProductDetail(string colorcode);
        Task<Color> GetColorById(Guid id);

        void AddOrUpdateColor(ColorViewModel model);

        bool ExistColorName(ColorViewModel model);
        bool ExistColorCode(ColorViewModel model);

        void RemoveColor(Guid id);

        #endregion

        #region Theme
        Task<Theme> GetTheme();

        void UpdateLogoImage(LogoViewModel model);
        #endregion

        #region City
        Task<List<City>> GetStates();

        Task<City> GetStateById(Guid id);


        bool ExistStateName(CityViewModel model);

        void AddAndUpdateState(CityViewModel model);



        bool ExistStateNameExcel(string name);
        void AddExcelState(CityViewModel model);

        void RemoveState(Guid id);

        Task<List<City>> GetCities(Guid id);
        void AddAndUpdateCity(CityViewModel model);
        bool ExistCityName(CityViewModel model);

        bool ExistCityNameExcel(Guid Id, string name);

        void AddExcelCity(CityViewModel model);

        Guid? RemoveCity(Guid id);

        #endregion

    }
}
