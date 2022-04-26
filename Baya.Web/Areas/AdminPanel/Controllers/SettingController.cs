using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Baya.Core.InterFaces;
using Microsoft.AspNetCore.Mvc;
using Baya.Core.ViewModels;

namespace Baya.Web.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    //[RoleAttribute("Admin")]
    public class SettingController : Controller
    {
        private IAdmin _admin;

        public SettingController(IAdmin admin)
        {
            _admin = admin;
        }
        [Route("/Admin/SiteSetting")]
        public async Task<IActionResult> SiteSetting()
        {
            var result = await _admin.GetSetting();
            SiteSettingViewModel sitesetting = new SiteSettingViewModel()
            {
                Desc = result.Desc,
                Fax = result.Fax,
                Name = result.Name,
                Tel = result.Tel,
                About = result.About,
                Terms = result.Terms
            };
            ViewBag.IsSuccess = false;
            return View(sitesetting);
        }

        [Route("/Admin/SiteSetting")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SiteSetting(SiteSettingViewModel viewModel)
        {
            if (_admin.UpdateSiteSetting(viewModel))
            {
                ViewBag.IsSuccess = true;
                return View(viewModel);

            }
            else
            {
                return NotFound();
            }



        }
    }
}
