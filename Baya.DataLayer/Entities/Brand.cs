using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Baya.DataLayer.Entities
{
  public  class Brand
    {
        [Key]
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        //[Display(Name = "نام برند")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        //[MaxLength(15, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        //[MinLength(3, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد.")]
        //[RegularExpression(@"^[\u0622\u0627\u0626\u0628\u067E\u062A-\u062C\u0686\u062D-\u0632\u0698\u0633-\u063A\u0641\u0642\u06A9\u06AF\u0644-\u0648\u06CC]+$", ErrorMessage = "فقط حروف فارسی قابل قبول است")]
        public string BrandName { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }



    }
}
