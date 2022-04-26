using Baya.Core.InterFaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baya.Web.Components
{
    public class OffProductComponent : ViewComponent
    {
        private readonly IProduct _product;

        public OffProductComponent(IProduct product)
        {
            _product = product;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var list = await _product.GetOffProducts();
            //return View("_MainProduct.cshtml", list);
            return View("/Views/Shared/_OffProduct.cshtml", list);
        }
    }
}
