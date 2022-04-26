using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Baya.DataLayer.Entities
{
  public class Role
    {
        public Guid RoleId { get; set; }


        [Display(Name = "نام لاتین نقش")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "فقط حروف انگلیسی قابل قبول است")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(15, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        [MinLength(3, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد.")]
        public string RoleName { get; set; }



        [Display(Name = "نام فارسی نقش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(15, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        [MinLength(3, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد.")]
        [RegularExpression(@"^[\u0622\u0627\u0626\u0628\u067E\u062A-\u062C\u0686\u062D-\u0632\u0698\u0633-\u063A\u0641\u0642\u06A9\u06AF\u0644-\u0648\u06CC]+$", ErrorMessage = "فقط حروف فارسی قابل قبول است")]
        public string RoleTitle { get; set; }
    }
}
