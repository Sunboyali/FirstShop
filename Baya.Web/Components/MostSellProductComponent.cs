using Baya.Core.InterFaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baya.Web.Components
{
    public class MostSellProductComponent : ViewComponent
    {
        private readonly IProduct _product;

        public MostSellProductComponent(IProduct product)
        {
            _product = product;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            var list = await _product.GetMostSellProducts();
            //return View("_MainProduct.cshtml", list);
            return View("/Views/Shared/_MostSellProduct.cshtml", list);
        }
    }
}
