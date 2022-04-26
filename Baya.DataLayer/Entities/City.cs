using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Baya.DataLayer.Entities
{
    public class City
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        [ForeignKey("State")]
        public Guid? StateId { get; set; }


        #region Relations
        public virtual City State { get; set; }

        #endregion

    }






}
