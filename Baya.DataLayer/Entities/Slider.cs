using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Baya.DataLayer.Entities
{
   public class Slider
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }

        public bool IsActive { get; set; }
        public virtual ICollection<SliderImage> SliderImages { get; set; }
    }



    public class SliderImage
    {
        [Key]
        public Guid Id { get; set; }
        public Guid SliderId { get; set; }

        public string ImageName { get; set; }
        public string AltImage { get; set; }
        public string TitleImage { get; set; }
        public string Link { get; set; }
        public int DisplaySlide { get; set; }

        [ForeignKey("SliderId")]
        public virtual Slider Slider { get; set; }

    }
}
