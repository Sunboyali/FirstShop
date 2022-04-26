using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Baya.Core.ViewModels
{

    public class ShowFastProductViewModel
    {
        public Guid Id { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductImg { get; set; }
        public int Price { get; set; }
        public int CountProduct { get; set; }
        public int VisitCount { get; set; }
        public int Off { get; set; }
        
    }

    


    public class ShowCardViewModel
    {
        public Guid OrderDetailId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ImageName { get; set; }
        public int CountProduct { get; set; }
        public int Price { get; set; }
        public long SumPrice { get; set; }
    }
}
