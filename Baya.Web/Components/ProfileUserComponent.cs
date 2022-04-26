using Baya.Core.InterFaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Baya.Web.Components 
{
   
    public class ProfileUserComponent : ViewComponent
    {
        private readonly IUser _user;

        public ProfileUserComponent(IUser user)
        {
            _user = user;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            string CurrentUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _user.GetUserForProfile(Guid.Parse(CurrentUserId));

            return View("_ShowProfile.cshtml", user);


        }


    }
}
