using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Baya.DataLayer.Entities
{
  public class Product
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }

        public string Color { get; set; }

        [Display(Name = "کد محصول")]
        [Required][MaxLength(8)]

        public string ProductCode { get; set; }

        [Display(Name = "نام محصول")]
        [Required]
        [MaxLength(100)]
        public string ProductName { get; set; }

        [Display(Name = "تصویر محصول")]
        public string ProductImg { get; set; }

       

        [Display(Name = "مبلغ محصول")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید ")]
        public int Price { get; set; }


        public int FinalPrice { get; set; } // Price - Off


        [Display(Name = "نمایش محصول")]
        public bool IsShowProduct { get; set; }

        [Display(Name = "موجودی")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید ")]
        public int CountProduct { get; set; }



        [Display(Name = "شگفت انگیز")]
        public bool Amazing { get; set; }

        [Display(Name = "تخفیف")]
        public int Off { get; set; }



        public int CountsSale { get; set; }

        //[Display(Name = "حذف شده؟")]
        //public bool IsDelete { get; set; }

        #region Relations


        [ForeignKey(nameof(CategoryId))]
        public virtual Category Category { get; set; }

        public virtual ProductDetail ProductDetail { get; set; }
        public virtual ICollection<ProductGallery> ProductGalleries { get; set; }

        #endregion

    }

    public class Color
    {
        public Guid ColorId { get; set; }

        public string ColorName { get; set; }
        public string ColorCode { get; set; }
       
    }
    public class ProductGallery
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }

        public string ImageName { get; set; }

        public string Alt { get; set; }



        #region Relation
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        #endregion

    }

}
