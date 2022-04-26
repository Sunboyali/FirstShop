using Baya.DataLayer.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Baya.Core.ViewModels
{
    #region Category
    //public class CategoryTreeViewNode
    //{
    //    public string id { get; set; }
    //    public string parent { get; set; }
    //    public string text { get; set; }
    //    public string icon { get; set; }
    //}


    public class MainCategoryViewModel
    {
        public Guid Id { get; set; }      

        public string CategoryName { get; set; }

        public IFormFile MainCatImg { get; set; }

        public string CategoryImageName { get; set; }

    }

    public class SubCategoryViewModel
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; }
        public Guid? ParentId { get; set; }
        public byte Level { get; set; }
    }


    #endregion

    #region Product
    public class ProductViewModel
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public string[] Color { get; set; }
        public string ProductName { get; set; }

        public IFormFile ImageProduct { get; set; }

        public IEnumerable<IFormFile> ImagesGallery { get; set; }
        public string ProductImgName { get; set; }
        public decimal Price { get; set; }
        public int CountProduct { get; set; }
        public int Off { get; set; }

        [DataType(DataType.MultilineText)]
        public string DesProduct { get; set; }
        public string Feature { get; set; }
        public string MetaTag { get; set; }
        public string MetaDescription { get; set; }
        public bool Amazing { get; set; }

    }

    public class ProductGalleryViewModel
    {
        public Guid Id { get; set; }

        public IEnumerable<IFormFile> GalleryImages { get; set; }
        public List<ProductGallery> GalleryImagesName { get; set; }

    }

    public class UpdateImageViewModel
    {
        public Guid Id { get; set; }
        public IFormFile SingleImageProduct { get; set; }
    }


    public class DetailProductViewModel
    {
        public string Category { get; set; }
        public string Color { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductImgName { get; set; }
        public decimal Price { get; set; }
        public decimal FinalPrice { get; set; }
        public int CountProduct { get; set; }
        public int Off { get; set; }
        public string DesProduct { get; set; }
        public string Feature { get; set; }
        public string MetaTag { get; set; }
        public string MetaDescription { get; set; }
        public bool Amazing { get; set; }
        public bool IsShowProduct { get; set; }

        public string DateAndTime { get; set; }

    }
    #endregion

    #region Role
    public class RoleViewModel
    {
        public Guid Id { get; set; }

        public string RoleName { get; set; }
        public string RoleTitle { get; set; }
    }
    #endregion

    #region User
    public class UserViewModel
    {

        public Guid Id { get; set; }
        public Guid RoleId { get; set; }      
        public string Mobile { get; set; }       
        public string Password { get; set; }
    }

    public class ChangeRoleViewModel
    {
        public Guid Id { get; set; }
        public Guid RoleId { get; set; }
    }
    public class UserDetailViewModel
    {
        public string Mobile { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public string EmailUser { get; set; }
        public string Address { get; set; }
        public string NationalCode { get; set; }
        public string PostCode { get; set; }
    }


    #endregion
          
    #region Poll
    public class PollViewModel
    {
        public Guid Id { get; set; }
        public string Question { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Answer { get; set; }
        public string[] AnswerList { get; set; }
        public List<PollOption> AnswerListEdit { get; set; }
        public bool Active { get; set; }

    }
    #endregion

    #region Slider

    public class SlideImageViewModel
    {

        public Guid Id { get; set; }
        public Guid SlideId { get; set; }
        public IFormFile ImageSlide { get; set; }
        public string ImageName { get; set; }
        public string AltImage { get; set; }
        public string TitleImage { get; set; }
        public string Link { get; set; }
        public int DisplaySlide { get; set; }
    }
    public class AddSlideImageViewModel
    {

        public Guid Id { get; set; }       
        public IFormFile ImageSlide { get; set; }
        public string ImageName { get; set; }      
        public string AltImage { get; set; }       
        public string TitleImage { get; set; }        
        public string Link { get; set; }      
        public int DisplaySlide { get; set; }
    }

    public class UpdateSlideImageViewModel
    {
        [Display(Name = "تصویر اسلایدر")]


        public IFormFile Image { get; set; }
        public string ImageName { get; set; }

        [Display(Name = "نام جایگزین")]

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string AltImage { get; set; }


        [Display(Name = "متن نمایشی")]
        public string TitleImage { get; set; }
        [Display(Name = "آدرس لینک")]
        public string Link { get; set; }

        [Display(Name = "شماره اسلاید")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int DisplaySlide { get; set; }


    }
    #endregion



    #region Banner
    public class BannerImageViewModel
    {
        public Guid Id { get; set; }
        public IFormFile Image { get; set; }
        public string BannerName { get; set; }       
        public string ImageName { get; set; }       
        public string AltImage { get; set; }       
        public string TitleImage { get; set; }        
        public string Link { get; set; }
    }
    #endregion

    #region Color
    public class ColorViewModel
    {
        public Guid Id { get; set; }

        public string ColorName { get; set; }
        public string ColorCode { get; set; }
    }
    #endregion

    #region Setting
    public class SiteSettingViewModel
    {
        [Display(Name = "نام سایت")]
        [MaxLength(100, ErrorMessage = "مقدار {0} نباید بیش تر از {1} کاراکتر باشد")]
        public string Name { get; set; }

        [Display(Name = "توضیحات سایت")]
        [MaxLength(200, ErrorMessage = "مقدار {0} نباید بیش تر از {1} کاراکتر باشد")]
        public string Desc { get; set; }

        [Display(Name = "درباره ما")]
        [DataType(DataType.MultilineText)]
        public string About { get; set; }

        [Display(Name = "شرایط و قوانین")]
        [DataType(DataType.MultilineText)]
        public string Terms { get; set; }

        [Display(Name = "شماره تماس")]
        [MaxLength(40, ErrorMessage = "مقدار {0} نباید بیش تر از {1} کاراکتر باشد")]
        public string Tel { get; set; }

        [Display(Name = "شماره فکس")]
        [MaxLength(40, ErrorMessage = "مقدار {0} نباید بیش تر از {1} کاراکتر باشد")]
        public string Fax { get; set; }

    }
    #endregion

    #region Theme
    public class LogoViewModel
    {
        public Guid Id { get; set; }
        public IFormFile LogoImage { get; set; }
        public string ImageLogoName { get; set; }
    }


    public class AllModelsThemeViewModel
    {
        public LogoViewModel LogoModel { get; set; }
    }


    #endregion

    #region City
   public class CityViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }     
        public Guid? StateId { get; set; }
    }
    #endregion






}
