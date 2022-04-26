using Baya.Core.InterFaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Baya.Web.Components
{
    public class FavoriteComponent : ViewComponent
    {
        private readonly IScope _scope;

        public FavoriteComponent(IScope scope)
        {
            _scope = scope;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {

            string CurrentUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var listfavorite = await _scope.GetFavoritesUser(Guid.Parse(CurrentUserId));
            return View("_ShowFavorites.cshtml", listfavorite);


        }
    }
}
