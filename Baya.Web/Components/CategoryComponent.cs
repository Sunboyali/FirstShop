using Baya.Core.InterFaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baya.Web.Components
{
    public class CategoryComponent : ViewComponent
    {
        private readonly IScope _scope;

        public CategoryComponent(IScope scope)
        {
            _scope = scope;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            var catlist = await _scope.ShowCategory();
            return View("/Views/Shared/_MainCategory.cshtml", catlist);
        }
    }
}
