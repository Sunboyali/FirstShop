using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Baya.Core.ViewModels;
using Baya.Core.InterFaces;


namespace Baya.Web.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    //[RoleAttribute("Admin")]
    public class RoleController : Controller
    {
        private IAdmin _admin;
        public RoleController(IAdmin admin)
        {
            _admin = admin;
        }

        [Route("/Admin/Roles")]
        public async Task<IActionResult> Index()
        {
            var list = await _admin.GetRoles();
            return View(list);
        }
        public async Task<IActionResult> AddOrEditRole(Guid Id)
        {
            if (Id == Guid.Empty)
                return View(new RoleViewModel());

            //else

            var Model = await _admin.GetRoleById(Id);
            if (Model == null)
                return NotFound();
            RoleViewModel viewModel = new RoleViewModel()
            {
                Id = Model.RoleId,
                RoleName = Model.RoleName,
                RoleTitle = Model.RoleTitle,
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddAndUpdateRole(RoleViewModel model)
        {
            if (model.RoleName == null)
                return Json(0);
            else if (model.RoleTitle == null)
                return Json(1);
            else if (_admin.ExistRoleName(model))
                return Json(2);
            _admin.AddAndUpdateRole(model);
            return Json(new { redirectToUrl = Url.Action("Index", "Role") });

        }




        public IActionResult Delete(Guid id)
        {
            _admin.DeleteRole(id);
            return RedirectToAction(nameof(Index));

        }
    }
}
