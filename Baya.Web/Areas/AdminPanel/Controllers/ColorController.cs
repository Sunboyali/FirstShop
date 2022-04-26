using Baya.Core.InterFaces;
using Baya.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baya.Web.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    //[RoleAttribute("Admin")]
    public class ColorController : Controller
    {
        private readonly IAdmin _admin;

        public ColorController(IAdmin admin)
        {
            _admin = admin;
        }


        [Route("/Admin/Colors")]
        public async Task<IActionResult>  Index()
        {
            var colorlist = await _admin.GetColors();
            return View(colorlist);
        }


        public async Task<IActionResult> AddOrEditColor(Guid id)
        {
            if (id == Guid.Empty)
                return View(new ColorViewModel());
            //else
            var color = await _admin.GetColorById(id);
            ColorViewModel model = new ColorViewModel()
            {
                ColorCode = color.ColorCode,
                ColorName = color.ColorName,
            };
            return View(model);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddAndUpdateColor(ColorViewModel model)
        {
            if (model.ColorName == null)
                return Json(0);
            else if (model.ColorCode == null)
                return Json(1);

            else if (_admin.ExistColorName(model))
                return Json(2);

            else if (_admin.ExistColorCode(model))
                return Json(3);

            _admin.AddOrUpdateColor(model);
            return Json(new { redirectToUrl = Url.Action("Index", "Color") });

        }

        public IActionResult Delete(Guid Id)
        {
            _admin.RemoveColor(Id);
            return RedirectToAction(nameof(Index));
        }


    }
}
