using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Baya.Core.Classes;
using Baya.Core.InterFaces;
using System.Security.Claims;
using Baya.Core.ViewModels;

namespace Baya.Web.Controllers
{
    //[RoleAttribute("User")]
    public class ProfileController : Controller
    {
        private readonly IUser _user;

        public ProfileController(IUser user)
        {
            _user = user;
        }

        [Route("Profile")]
        public async Task<IActionResult> ProfileManage()
        {
            string CurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _user.GetUserById(Guid.Parse(CurrentUserId));

            UpdateProfileViewModel model = new UpdateProfileViewModel()
            {
                EmailUser = user.EmailUser,
                Name = user.Name,
                Family = user.Family,
                UserImageName = user.UserImg,
                NationalCode = user.NationalCode,
            };

            ViewBag.ErrorOne = false;
            ViewBag.ErrorTwo = false;
            ViewBag.ErrorThree = false;
           
            return View(model);
        }


        //[Route("Profile")]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ProfileManage(UpdateProfileViewModel viewModel)
        //{
        //    string CurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    var user = await _user.GetUserById(Guid.Parse(CurrentUserId));
        //    if (ModelState.IsValid)
        //    {
                
               
        //        int result = _user.UpdateProfileUser(Guid.Parse(CurrentUserId), viewModel);

        //        if(result == 1)
        //        {
        //            return NotFound();
        //        }
        //        else if(result== 2) // فرمت تصویر مناسب نیست
        //        {
        //            ViewBag.ErrorOne = true;

        //        }else if(result == 3) // فایل انتخابی تصویر نیست
        //        {
        //            ViewBag.ErrorTwo = true;
        //        }
        //        else
        //        {
        //            ViewBag.ErrorThree = true;
        //        }


        //    }
        //    ViewBag.img = user.UserImg;
        //    return View(viewModel);
        //}
    }
}
