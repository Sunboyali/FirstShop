using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Baya.Core.Classes;
using Baya.Core.InterFaces;
using Baya.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Baya.Web.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    //[RoleAttribute("Admin")]
    public class BannerController : Controller
    {
        private readonly IAdmin _admin;

        public BannerController(IAdmin admin)
        {
            _admin = admin;
        }

        [Route("/Admin/Banners")]
        public async Task<IActionResult> Index()
        {
            var list = await _admin.GetBanners();
            return View(list);
        }



        public async Task<IActionResult> EditBanner(Guid Id)
        {
            var banner = await _admin.GetBannerById(Id);         

            BannerImageViewModel model = new BannerImageViewModel()
            {
                Id = banner.Id,
                BannerName = banner.Name,
                ImageName = banner.ImageName,
                AltImage = banner.AltImage,
                Link = banner.Link,              
                TitleImage = banner.TitleImage,
            };


            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateBanner(BannerImageViewModel model)
        {         
                       

            if (model.Image != null)
            {

                string strExt = Path.GetExtension(model.Image.FileName);

                if (strExt != ".jpg" && strExt != ".jpeg" && strExt != ".png")
                {
                    return Json(1);
                }
                else if (!ImageValidator.IsImage(model.Image)) //چک کردن تصویر
                {
                    return Json(2);
                }

            }
            _admin.UpdateBanner(model);
            return Json(new { redirectToUrl = Url.Action("Index", "Banner")});
        }   

    }
}
