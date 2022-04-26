using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Baya.Core.ViewModels
{
   public class UserProfileViewModel
    {
        public string Date { get; set; }
        public string Time { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public string ImageUser { get; set; }
        public long Wallet { get; set; }
    }

    public class UserFavoriteViewModel
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ProductImage { get; set; }
        public int ProductPrice { get; set; }
    }

    public class UpdateProfileViewModel
    {
      
        [Display(Name = "نام")]
        [MaxLength(30, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Name { get; set; }
        [Display(Name = "نام خانوادگی")]
        [MaxLength(30, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Family { get; set; }
        [Display(Name = "ایمیل")]
        [MaxLength(30, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string EmailUser { get; set; }
        [Display(Name = "آدرس")]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }


        public IFormFile ImageUser { get; set; }

        [Display(Name = "تصویر کاربر")]      

        public string UserImageName { get; set; }

        [Display(Name = "کد ملی کاربر")]
        public string NationalCode { get; set; }


        [Display(Name = "کد پستی کاربر")]
        public string PostCode { get; set; }
    }

    public class AddAddressInfoViewModel
    {
        public string Name { get; set; }       
        public string Family { get; set; }    
        
        public string Mobile { get; set; }

       
        public string State { get; set; }
       
        public string City { get; set; }       
        public string Address { get; set; }


        public string NationalCode { get; set; }

        public string PostCode { get; set; }

       
        public string Pelak { get; set; }
        public string Vahed { get; set; }

    }


}
