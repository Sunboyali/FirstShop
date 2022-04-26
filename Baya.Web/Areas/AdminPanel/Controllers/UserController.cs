using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Baya.Core.Classes;
using Baya.Core.InterFaces;
using Baya.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Baya.Web.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class UserController : Controller
    {
        private IAdmin _admin;
        public UserController(IAdmin admin)
        {
            _admin = admin;
        }
        [Route("/Admin/Users")]
        public async Task<IActionResult> Index(string mobile, int page = 1)
        {
            int countuser = 40;
            int skip = (page - 1) * countuser;
            int totalcount = 0;

            if (mobile != null)
            {
                ViewBag.Mobile = mobile;
                totalcount = _admin.CountActiveUsersBySearch(mobile);
                double remain = totalcount % countuser;
                ViewBag.pageId = page;
                if (remain == 0)
                {
                    ViewBag.PageCount = totalcount / countuser;
                }
                else
                {
                    ViewBag.PageCount = (totalcount / countuser) + 1;
                }
                var userlist = await _admin.GetActiveUsersBySearch(mobile,skip, countuser);
                return View(userlist);
            }
            else
            {
                
                totalcount = _admin.CountActiveUsers();
                double remain = totalcount % countuser;
                ViewBag.pageId = page;
                if (remain == 0)
                {
                    ViewBag.PageCount = totalcount / countuser;
                }
                else
                {
                    ViewBag.PageCount = (totalcount / countuser) + 1;
                }
                var userlist = await _admin.GetActiveUsers(skip, countuser);
                return View(userlist);
            }
           
        }
        [Route("/Admin/DeactivUsers")]
        public async Task<IActionResult> Deactive(string mobile,int page = 1)
        {
            int countuser = 40;
            int skip = (page - 1) * countuser;
            int totalcount = 0;

            if (mobile != null)
            {
                ViewBag.Mobile = mobile;
                totalcount = _admin.CountDeActiveUsersBySearch(mobile);
                double remain = totalcount % countuser;
                ViewBag.pageId = page;
                if (remain == 0)
                {
                    ViewBag.PageCount = totalcount / countuser;
                }
                else
                {
                    ViewBag.PageCount = (totalcount / countuser) + 1;
                }

                var userlist = await _admin.GetDeActiveUsersBySearch(mobile,skip, countuser);
                return View(userlist);
            }
            else
            {
                totalcount = _admin.CountActiveUsers();
                double remain = totalcount % countuser;
                ViewBag.pageId = page;
                if (remain == 0)
                {
                    ViewBag.PageCount = totalcount / countuser;
                }
                else
                {
                    ViewBag.PageCount = (totalcount / countuser) + 1;
                }

                var userlist = await _admin.GetDeActiveUsers(skip, countuser);
                return View(userlist);
            }

           
        }

        [Route("/Admin/BlockedUsers")]
        public async Task<IActionResult> Blocked(string mobile, int page = 1)
        {
            int countuser = 40;
            int skip = (page - 1) * countuser;
            int totalcount = 0;
            if (mobile != null)
            {
                ViewBag.Mobile = mobile;
                totalcount = _admin.CountBlockedUsersBySearch(mobile);
                double remain = totalcount % countuser;
                ViewBag.pageId = page;
                if (remain == 0)
                {
                    ViewBag.PageCount = totalcount / countuser;
                }
                else
                {
                    ViewBag.PageCount = (totalcount / countuser) + 1;
                }
                var userlist = await _admin.GetBlockedUsersBySearch(mobile,skip, countuser);
                return View(userlist);
            }
            else
            {
                totalcount = _admin.CountBlockedUsers();
                double remain = totalcount % countuser;
                ViewBag.pageId = page;
                if (remain == 0)
                {
                    ViewBag.PageCount = totalcount / countuser;
                }
                else
                {
                    ViewBag.PageCount = (totalcount / countuser) + 1;
                }
                var userlist = await _admin.GetBlockedUsers(skip, countuser);
                return View(userlist);
            }

            
        }



        public async Task<IActionResult> AddUser()
        {
            var roles = await _admin.GetRoles();
            ViewBag.Roles = new SelectList(roles, "RoleId", "RoleTitle");
            return View(new UserViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddNewUser(UserViewModel model)
        {
            if (model.Mobile == null)
            {
                return Json(0);
            }
            else if (model.Password == null)
            {
                return Json(1);
            }
            else if (_admin.CheckMobileNumber(model.Mobile))
            {
                return Json(2);
            }
            _admin.AddUser(model);

            return Json(new { redirectToUrl = Url.Action("Index", "User") });
        }

        public async Task<IActionResult> ChangeRole(Guid Id,Guid RoleId)
        {
            var roles = await _admin.GetRoles();
            ViewBag.Roles = new SelectList(roles, "RoleId", "RoleTitle",RoleId);       

            return View();
        }


        [HttpPost]
        public IActionResult ChangeRoleUser(ChangeRoleViewModel model)
        {
            _admin.UpdateRoleUser(model);
            return RedirectToAction(nameof(Index));
        } 

      
        
        public async Task<IActionResult> UserDetail(Guid Id)
        {
            var user = await _admin.GetUserById(Id);

            UserDetailViewModel model = new UserDetailViewModel()
            {
                Mobile = user.Mobile,
                Date = user.UserDetail.Date,
                EmailUser = user.UserDetail.EmailUser,
                Name = user.UserDetail.Name,
                Family = user.UserDetail.Family,
                NationalCode = user.UserDetail.NationalCode,
                Time = user.UserDetail.Time,
            };
            return View(model);

        }

        public IActionResult BlockUser(Guid id)
        {
            _admin.BlockUser(id);
            return RedirectToAction(nameof(Index));

        }    

    }
}
