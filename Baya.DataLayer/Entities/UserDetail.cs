using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Baya.DataLayer.Entities
{
  public class UserDetail
    {
        [Key]
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        [Display(Name = "تاریخ عضویت")]
        [MaxLength(10, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string Date { get; set; }
        [Display(Name = "ساعت عضویت")]
        [MaxLength(10, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string Time { get; set; }
        [Display(Name = "نام")]
        [MaxLength(30, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string Name { get; set; }


        [Display(Name = "نام خانوادگی")]
        [MaxLength(30, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string Family { get; set; }

        [Display(Name = "ایمیل")]
        [MaxLength(30, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string EmailUser { get; set; } 

       
        [Display(Name = "تصویر کاربر")]
        [MaxLength(30, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]

        public string UserImg { get; set; }

        [Display(Name = "کد ملی کاربر")]
        public string NationalCode { get; set; }


      

        #region Relation

        public virtual User User { get; set; }
        #endregion
    }

    public class UserAddress
    {
        public Guid Id { get; set; }


        public Guid UserId { get; set; }



        [Display(Name = "نام")]
        [MaxLength(30, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string TransfereeName { get; set; }


        [Display(Name = "نام خانوادگی")]
        [MaxLength(30, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string TransfereeFamily { get; set; }
        [Display(Name = "استان")]
        public string Ostan { get; set; }
        [Display(Name = "شهر")]
        public string City { get; set; }

        [Display(Name = "آدرس")]
        public string HomeAddress { get; set; }

        [Display(Name = "پلاک")]
        public string Pelak { get; set; }

        [Display(Name = "واحد")]
        public string Vahed { get; set; }

        [Display(Name = "کد پستی کاربر")]
        public string PostCode { get; set; }

        public string Mobile { get; set; }
        public string HomeTel { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
