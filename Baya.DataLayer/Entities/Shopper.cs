using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Baya.DataLayer.Entities
{
  public class Shopper
    {
        [Key]
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        [Display(Name = "کد ملی")]
        [MaxLength(10)]
        
       
        public string NationalCode { get; set; }

        [Display(Name = "شماره تماس ثابت")]
        [MaxLength(30, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]

        public string Tel { get; set; }

        [Display(Name = "آدرس سکونت")]
        [DataType(DataType.MultilineText)]
        public string AddressHome { get; set; }


        [Display(Name = "آدرس فروشگاه")]
        [DataType(DataType.MultilineText)]
        public string ShopAddress { get; set; }



        [Display(Name = "تصویر جواز کسب")]
        [MaxLength(30, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]

        public string Img { get; set; }


        public bool IsConfirm { get; set; }


        #region Relation
        public virtual User User { get; set; }
        #endregion
    }
}
