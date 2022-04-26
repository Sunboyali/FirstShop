using Baya.Core.InterFaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Baya.Core.ViewModels;


namespace Baya.Web.Components
{
    public class SlidersComponent : ViewComponent
    {
        private readonly IScope _scope;

        public SlidersComponent(IScope scope)
        {
            _scope = scope;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            SliderBannerViewModel model = new SliderBannerViewModel();

            model.SliderImageModel = await _scope.GetSlidersImage();

            model.BannerImageModel = await _scope.GetBannersImage();

            return View("/Views/Shared/_Sliders.cshtml", model);

        }
    }
}
