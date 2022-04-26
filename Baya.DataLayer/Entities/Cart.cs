using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Baya.DataLayer.Entities
{
   public class Order
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }       

        public int CountOrders { get; set; }
        public string CreateDate { get; set; }
        public string CreateTime { get; set; }
        public long Sum { get; set; }

        public bool IsFinally { get; set; }


        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }

    public class OrderDetail
    {
        [Key]
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int Price { get; set; }
        public int Count { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}
