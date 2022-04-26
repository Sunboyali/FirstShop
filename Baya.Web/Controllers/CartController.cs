using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Baya.Core.Classes;
using Baya.Core.InterFaces;
using Baya.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Baya.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly IScope _scope;
       
        public string CurrentUserId { get; set; }


        public CartController(IScope scope)
        {
            _scope = scope;
        }


        [Route("Cart")]
        public async Task<IActionResult> ShowBasket()
        {
            CurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (CurrentUserId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var listbasket = await _scope.ShowBasketUser(Guid.Parse(CurrentUserId));
            return View(listbasket);
        }




        public async Task<IActionResult> DeleteFinal(Guid id)
        {
            CurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orderdetail = await _scope.GetOrderDetailById(id);

            long sum = orderdetail.Order.Sum;
            int odd = orderdetail.Count * orderdetail.Price;
            long totalprice = (Int32)sum - odd;


            _scope.DeleteBasketProduct(totalprice, id);



            int count = _scope.CountCartUser(Guid.Parse(CurrentUserId));
            return RedirectToAction("ShowBasket");
        }


        public IActionResult AddCard(Guid id)
        {
            if (User.Identity.Name == null)
            {
                return Json(new { status = "notlogin" });
            }

            CurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            _scope.AddCard(id, (Guid.Parse(CurrentUserId)));
            int count = _scope.CountCartUser(Guid.Parse(CurrentUserId));

            return Json(new { status = "added", result = count });
        }



        public IActionResult AddFavorite(Guid id)
        {
            if (User.Identity.Name == null)
            {
                return Json(new { status = "notlogin" });
            }
            CurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool fav = _scope.AddFavorite(id, Guid.Parse(CurrentUserId));


            if (fav == true)
            {
                int count = _scope.CountFavoritesUser(Guid.Parse(CurrentUserId));
                //return ViewComponent("FavoriteComponent");
                return Json(new { status = "added", result = count });
            }
            return Json(new { status = "removed" });
        }


        public async Task<IActionResult> DeleteBasketProduct(Guid id)
        {
            CurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orderdetail = await _scope.GetOrderDetailById(id);

            long sum = orderdetail.Order.Sum;
            int odd = orderdetail.Count * orderdetail.Price;
            long totalprice = (Int32)sum - odd;


            _scope.DeleteBasketProduct(totalprice, id);



            int count = _scope.CountCartUser(Guid.Parse(CurrentUserId));

            return Json(new { status = "success", result = totalprice.ToString("#,0 تومان"), result2 = count });
        }

        public IActionResult DeleteFavorite(Guid id)
        {
            CurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            _scope.DeleteFavorite(id);
            int count = _scope.CountFavoritesUser(Guid.Parse(CurrentUserId));

            return Json(new { status = "success", result = count });

        }


        [Route("Shopping")]
        public IActionResult Shopping()
        {
            CurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!_scope.UserHaveAddress(Guid.Parse(CurrentUserId)))
            {
                return RedirectToAction("Addresses");
            }
            return View();



            //var user = await _scope.GetInformationUserForPayment(Guid.Parse(CurrentUserId));
            //var states = await _scope.GetCities();
            //Guid stateid = Guid.Empty;
            //Guid cityid = Guid.Empty;
            //if (user.Ostan != null && user.City != null)
            //{
            //    stateid = _scope.GetStateIdByName(user.Ostan);
            //    cityid = _scope.GetCityIdByName(user.City);
            //    ViewBag.States = new SelectList(states.Where(c => c.StateId == null), "Id", "Name",stateid);
            //    ViewBag.Cities = new SelectList(states.Where(c => c.StateId == stateid), "Id", "Name", cityid);
            //}



           

            //ViewBag.States = new SelectList(states.Where(c => c.StateId == null), "Id", "Name");
            //UserInfoViewModel model = new UserInfoViewModel()
            //{
            //    Address = user.Address,
            //    EmailUser = user.EmailUser,
            //    Family = user.Family,
            //    Name = user.Name,
            //    HomeTel = user.HomeTel,
            //    NationalCode = user.NationalCode,
            //    PostCode = user.PostCode,
            //    State = stateid.ToString(),
            //    City = cityid.ToString(),
            //};

        }

        [Route("Addresses")]
        public async Task<IActionResult> Addresses()
        {
            AddAddressInfoViewModel model = new AddAddressInfoViewModel();
            CurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var states = await _scope.GetCities();
            ViewBag.States = new SelectList(states.Where(c => c.StateId == null), "Id", "Name");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddAddresses(AddAddressInfoViewModel model)
        {
            CurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (model.State == null)
            {
                return Json(0);
            }
            else if (model.City == null)
            {
                return Json(1);
            }
            else if (model.Address == null)
            {
                return Json(2);
            }
            else if (model.Pelak == null)
            {
                return Json(3);
            }
            else if (model.PostCode == null)
            {
                return Json(4);
            }
            else if (model.Name == null)
            {
                return Json(5);
            }
            else if (model.Family == null)
            {
                return Json(6);
            }
            else if (model.Mobile == null)
            {
                return Json(7);
            }

            if (model.PostCode.Length != 10)
            {
                return Json(8);
            }
            else if (!Regex.IsMatch(model.PostCode, @"^[0-9]{0,10}$"))
            {
                return Json(9);
            }

            if (model.Name.Length > 15)
            {
                return Json(10);
            }

           else if (!Regex.IsMatch(model.Name, @"^[\u0020\u0600-\u06FF]+$"))
            {
                return Json(11);
            }
            if (model.Family.Length > 20)
            {
                return Json(12);
            }

            else if (!Regex.IsMatch(model.Family, @"^[\u0020\u0600-\u06FF]+$"))
            {
                return Json(13);
            }
            if (model.Mobile.Length != 11)
            {
                return Json(14);
            }else if (!Regex.IsMatch(model.Mobile, @"^0?9\d{9}$"))
            {
                return Json(15);
            }

            //@"^\d$"
            //"^[آ-ی]$"
            //Regex.IsMatch(value, @ "^[\u0600-\u06FF]+$")

            _scope.AddAddressUser(Guid.Parse(CurrentUserId),model);

            return Json(new { redirectToUrl = Url.Action("Shopping", "Cart") });
        }





        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult FinalUserCart(UserInfoViewModel model)
        //{

        //    if (!CheckNationalCode.CheckCode(model.NationalCode))
        //    {
        //        return Json(0);
        //    }



        //    return null;
        //}


        public async Task<IActionResult> GetCities(Guid id)
        {
            var cities = await _scope.GetCities();
            return Json(new SelectList(cities.Where(c => c.StateId == id), "Id", "Name"));
        }


        public async Task<JsonResult> GetUserInfo()
        {
            CurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userinfo = await _scope.GetUserInfo(Guid.Parse(CurrentUserId));

            return Json(userinfo);
        }
    }
}
