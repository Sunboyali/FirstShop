using Baya.Core.Classes;
using Baya.Core.InterFaces;
using Baya.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Baya.Web.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    //[RoleAttribute("Admin")]
    public class ThemeController : Controller
    {
        private readonly IAdmin _admin;

        public ThemeController(IAdmin admin)
        {
            _admin = admin;
        }

        public async Task<IActionResult> Index()
        {
            var theme = await _admin.GetTheme();

            AllModelsThemeViewModel models = new AllModelsThemeViewModel();

            models.LogoModel = new LogoViewModel();

            models.LogoModel.ImageLogoName = theme.Logo;
            models.LogoModel.Id = theme.Id;


            return View(models);
        }

        [HttpPost]
        public IActionResult UpdateLogo(LogoViewModel model)
        {
            if (model.LogoImage == null)
            {
                return Json(0);
            }
            else
            {
                string strExt = Path.GetExtension(model.LogoImage.FileName);

                if (strExt != ".jpg" && strExt != ".jpeg" && strExt != ".png")
                {
                    return Json(1);
                }
                else if (!ImageValidator.IsImage(model.LogoImage)) //چک کردن تصویر
                {
                    return Json(2);
                }

                _admin.UpdateLogoImage(model);
                return Json(3);
            }

            
        }

    }
}
