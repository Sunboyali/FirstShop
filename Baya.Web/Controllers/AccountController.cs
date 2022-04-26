using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Baya.Core.ViewModels;
using Baya.Core.InterFaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace Baya.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUser _user;
       
        public AccountController(IUser user)
        {
            _user = user;
           
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(string mobile_User, string user_Password)
        {
            _user.RegisterUpdateUser(mobile_User, user_Password);
            var user = await _user.LoginUserAfterRegister(mobile_User);
            if (user != null)
            {
                var claims = new List<Claim>()
                        {
                            new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                            new Claim(ClaimTypes.Name,mobile_User.ToString()),
                        };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                var properties = new AuthenticationProperties
                {
                    IsPersistent = true

                };

               await HttpContext.SignInAsync(principal, properties);

                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Login(string mobile_User, string user_Password)
        {
            var user = await _user.LoginUser(mobile_User, user_Password);
            if (user != null)
            {
                var claims = new List<Claim>()
                        {
                            new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                            new Claim(ClaimTypes.Name,mobile_User.ToString()),
                        };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                var properties = new AuthenticationProperties
                {
                    IsPersistent = true

                };

               await HttpContext.SignInAsync(principal, properties);

                return RedirectToAction("Index", "Home");
            }
            return NotFound();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(string mobile_User, string user_Password)
        {
            _user.ChangePassUser(mobile_User, user_Password);
            var user = await _user.LoginUserAfterRegister(mobile_User);
            if (user != null)
            {
                var claims = new List<Claim>()
                        {
                            new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                            new Claim(ClaimTypes.Name,mobile_User.ToString()),
                        };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                var properties = new AuthenticationProperties
                {
                    IsPersistent = true

                };

                await HttpContext.SignInAsync(principal, properties);

                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }
        



        public JsonResult SendMobile(string Mobile)
        {
            System.Threading.Thread.Sleep(200);
            if (Mobile == null)
            {
                return Json(3);
            }
            if (_user.ExistMobileNumber(Mobile) == true)//Login
            {
                if (!_user.UserIsBlocked(Mobile))
                {
                    return Json(2);
                }
                else if (_user.UserIsActivate(Mobile))
                {
                    _user.RegisteredUserToActive(Mobile);//Sms Active Code
                    return Json(0);//Register
                }
                else
                {
                    return Json(1);
                }
               
            }
            else
            {
                _user.RegisterUser(Mobile);
                return Json(0);//Register
            }          
        }





        public JsonResult CheckActiveCode(string Mobile , string ActiveCode)
        {
            System.Threading.Thread.Sleep(200);
            if (!_user.UserIsActivate(Mobile, ActiveCode))
            {
                return Json(1);
            }
            return Json(544698);
        }


        public JsonResult CheckExistUser(string Mobile, string Password)
        {
            System.Threading.Thread.Sleep(200);
            if (!_user.ExistUser(Mobile, Password)) //Not Exist User
            {
                return Json(1);
            }
            return Json(0);
        }



        public JsonResult SendMobileToForget(string Mobile)
        {
            System.Threading.Thread.Sleep(200);
            if (Mobile == null)
            {
                return Json(1);
            }
            if (_user.ExistMobileNumberOfActive(Mobile))
            {
                if (!_user.UserIsBlocked(Mobile))
                {
                    return Json(2);
                }              
                else
                {
                    _user.RegisteredUserToActive(Mobile);//Send Sms Active Code
                    return Json(82456);
                }

            }
            else
            {
                return Json(0);
            }
        }


        public JsonResult SendAgainActiveCode(string Mobile)
        {
            if (Mobile != null)
            {
                _user.SendActiveCodeAgain(Mobile);
                return Json(21563);
            }
            return Json(0);
        }




        [Route("LogOut")]
        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }


    }
}
