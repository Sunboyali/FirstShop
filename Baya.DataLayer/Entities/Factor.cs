using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Baya.DataLayer.Entities
{
   public class Factor
    {
        [Key]

        [ForeignKey("Order")]
        public Guid OrderId { get; set; }

        public Guid UserId { get; set; }
       

        [Display(Name = "تاریخ")]
        [MaxLength(10)]
        public string Date { get; set; }

        [Display(Name = "ساعت")]
        [MaxLength(10)]
        public string Time { get; set; }

        [Display(Name = "شماره سفارش")]
        [MaxLength(8)]
        public string OrderNumber { get; set; }

        [Display(Name = "مبلغ کل")]
        public int TotalPrice { get; set; }

        [Display(Name = "تعداد محصول")]
        public int CountProduct { get; set; }

        [Display(Name = "شرح")]
        public string Desc { get; set; }

        [Display(Name = "درگاه")]
        public string BankName { get; set; }

        [Display(Name = "شماره پیگیری")]
        [MaxLength(50)]
        public string RefNumber { get; set; }

        [Display(Name = "شماره مرجع")]
        [MaxLength(50)]
        public string TraceNumber { get; set; }

       
       
        public virtual Order Order { get; set; }


    }
}
