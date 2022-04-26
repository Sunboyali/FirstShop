using Baya.DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Baya.Core.ViewModels
{
   public class SearchListModelViewModel
    {
        public IEnumerable<Product> ProductListSearch { get; set; }
        public IEnumerable<Category> ProductListSearchByCategory { get; set; }
    }
}
