using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Baya.DataLayer.Entities
{
   public class User
    {
        [Key]
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }   

      

        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200)]        
        public string Password { get; set; }

        [Display(Name = "موبایل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(11, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        [MinLength(11, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد.")]
        public string Mobile { get; set; }

        [Display(Name = "کد فعال سازی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(6, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        [MinLength(6, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد.")]
        public string ActiveCode { get; set; }





        [Display(Name = "وضعیت")]
        public bool IsActive { get; set; }

        public bool IsBlock { get; set; }      
       

        [Display(Name = "تعداد گزارش کاربر")]
        public int CountReoport { get; set; }

        [Display(Name = "توکن")]
        public string Token { get; set; }
        [Display(Name = "کیف پول")]
        public long Wallet { get; set; }

        #region Relation
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }

        public virtual UserDetail UserDetail { get; set; }

        public virtual ICollection<UserAddress> UserAddresses { get; set; }

        #endregion
    }
}
