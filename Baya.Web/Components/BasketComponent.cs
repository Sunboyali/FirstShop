using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Baya.Core.InterFaces;
using Microsoft.AspNetCore.Mvc;

namespace Baya.Web.Components
{

   
    public class BasketComponent : ViewComponent
    {
        private readonly IScope _scope;

        public BasketComponent(IScope scope)
        {
            _scope = scope;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
          
            string CurrentUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var listcard = await _scope.ShowBasketUser(Guid.Parse(CurrentUserId));
            return View("/Views/Shared/_ShowBasket.cshtml", listcard);

            //return await Task.FromResult((IViewComponentResult)View("_ShowBasket", listcard));



        }
    }
}
