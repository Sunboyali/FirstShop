using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Baya.DataLayer.Entities
{
  public  class ProductDetail
    {
        [Key]
        [ForeignKey("Product")]
        public Guid ProductId { get; set; }



        [Display(Name = "تاریخ ثبت محصول")]
        public string DateProduct { get; set; }


        [Display(Name = "ساعت ثبت محصول")]
        public string TimeProduct { get; set; }



        [Display(Name = "توضیحات محصول")]    
        public string Description { get; set; }

        [Display(Name = "ویژگی محصول")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Feature { get; set; }

        [Display(Name = "وزن محصول")]
        
        public string Weight { get; set; }

        [Display(Name = "تعداد بازدید")]
        public int VisitCount { get; set; }

        //SEO
        [Display(Name = "کلمات کلیدی")]
        public string MetaTag { get; set; }
        [Display(Name = "توضیحات متا")]
        public string MetaDescription { get; set; }

        #region Relation

        public virtual Product Product { get; set; }
        #endregion
    }
}
