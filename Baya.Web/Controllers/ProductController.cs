using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Baya.Core.InterFaces;
using System.Security.Claims;
using Baya.Core.ViewModels;

namespace Baya.Web.Controllers
{
    public class ProductController : Controller
    {

        private readonly IProduct _product;
        private readonly IScope _scope;


        public ProductController(IProduct product, IScope scope)
        {
            _product = product;
            _scope = scope;
        }




        public async Task<IActionResult> ShowFastProduct(Guid Id)
        {
            var SingleProduct = await _product.GetProductById(Id);

            ShowFastProductViewModel model = new ShowFastProductViewModel()
            {
                Id = Id,
                CountProduct= SingleProduct.CountProduct,
                Price = SingleProduct.Price,
                ProductCode = SingleProduct.ProductCode,
                ProductImg = SingleProduct.ProductImg,
                ProductName = SingleProduct.ProductName,
                VisitCount = SingleProduct.ProductDetail.VisitCount,
                Off = SingleProduct.Off,
            };


            return PartialView("_FastShowProduct", model);
        }



        [Route("Product/{id}/{name}")]
        public async Task<IActionResult> SingleProduct(Guid id,string name)
        {

            var product = await _product.ShowSingleProductById(id);


            return View(product);
        }

        [HttpPost]
        public IActionResult AddToCart(Guid id)
        {
            if (User.Identity.Name == null)
            {
                return Redirect("/LoginUser");
            }

            string CurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _scope.AddCard(id, (Guid.Parse(CurrentUserId)));
            return Redirect("/Cart");
        }

        [Route("/search")]
        public async Task<IActionResult> SearchProduct(string q,string sort = null, int page = 1)
        {
            int countproduct = 30;


            int skip = (page - 1) * countproduct;

            int totalcount = _product.CountProductSearch(q, sort);

            if (q != null && sort == null)
            {
               

                if (totalcount > countproduct)
                {
                    double remain = totalcount % countproduct;
                    ViewBag.pageId = page;
                    if (remain == 0)
                    {
                        ViewBag.PageCount = totalcount / countproduct;
                    }
                    else
                    {
                        ViewBag.PageCount = (totalcount / countproduct) + 1;
                    }

                    var searchProduct = await _product.SearchProduct(q, sort, skip, countproduct);
                    ViewBag.searchtext = q;

                    return View(searchProduct);
                }
                else
                {
                    var searchwithoutpageProduct = await _product.WithoutPage_SearchProduct(q, sort);
                    ViewBag.searchtext = q;

                    return View(searchwithoutpageProduct);
                }


            }
            else if (q != null && sort != null)
            {
              

                if (totalcount > countproduct)
                {
                    double remain = totalcount % countproduct;
                    ViewBag.pageId = page;
                    if (remain == 0)
                    {
                        ViewBag.PageCount = totalcount / countproduct;
                    }
                    else
                    {
                        ViewBag.PageCount = (totalcount / countproduct) + 1;
                    }

                    var searchProduct = await _product.SearchProduct(q, sort, skip, countproduct);
                    ViewBag.searchtext = q;
                    ViewBag.Sort = sort;

                    return View(searchProduct);

                }
                else
                {
                    var searchwithoutpageProduct = await _product.WithoutPage_SearchProduct(q, sort);
                    ViewBag.searchtext = q;
                    ViewBag.Sort = sort;

                    return View(searchwithoutpageProduct);

                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

           
        }
        public IActionResult GetTextSearch(string q,string sort)
        {
            return Json(new { redirectToUrl = Url.Action("SearchProduct", "Product", new { q = q , sort=sort}) });
        }


    }
}
