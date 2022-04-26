using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Baya.DataLayer.Entities
{
   public class Category
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("Parent")]
        public Guid? ParentId { get; set; }


      
        public string CategoryName { get; set; }

        public string CategoryImg { get; set; }
        public int CategoryLevel { get; set; }

        #region Relations
        public virtual Category Parent { get; set; }

        #endregion
    }



  
}
