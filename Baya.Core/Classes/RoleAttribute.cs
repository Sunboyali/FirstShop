using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;


namespace Baya.Core.Classes
{
   //public class RoleAttribute :AuthorizeAttribute, IAuthorizationFilter
   // {
   //     string _roleName;
   //     IUser  _iuser;
   //     public RoleAttribute(string roleName)
   //     {
   //         _roleName = roleName;
            
   //     }

   //     public void OnAuthorization(AuthorizationFilterContext context)
   //     {
   //         if (context.HttpContext.User.Identity.IsAuthenticated)
   //         {
   //             string userName = context.HttpContext.User.Identity.Name;
   //             _iuser = (IUser)context.HttpContext.RequestServices.GetService(typeof(IUser));
   //             if (!_iuser.ExistUserFromRole(_roleName, userName))
   //             {


   //                 context.Result = new RedirectResult("/Account/Login");
   //             }
   //         }
   //         else
   //         {
   //             context.Result = new RedirectResult("/Account/Login");
   //         }
   //     }
   // }
}
