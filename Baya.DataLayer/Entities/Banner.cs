using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Baya.DataLayer.Entities
{
   public class Banner
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ImageName { get; set; }
        public string AltImage { get; set; }
        public string TitleImage { get; set; }
        public string Link { get; set; }
        public bool IsActive { get; set; }
    }
}
