using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Baya.Core.ViewModels;
using Baya.Core.InterFaces;
using Baya.DataLayer.Entities;
using System.IO;
using Baya.Core.Classes;

namespace Baya.Web.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    //[RoleAttribute("Admin")]
    public class ProductController : Controller
    {
        private IAdmin _admin;
        public ProductController(IAdmin admin)
        {
            _admin = admin;
        }

        [Route("/Admin/Products")]
        public async Task<IActionResult> Index(string productcode, string productname, int page = 1)
        {
            int countproduct = 20;
            int totalcount = 0;
            int skip = (page - 1) * countproduct;
            if (productcode != null || productname != null)
            {
                totalcount = _admin.CountProductsBySearch(productcode, productname);
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
                ViewBag.Code = productcode;
                var list = await _admin.SearchProducts(productcode, productname, skip, countproduct);
                ViewBag.Name = productname;
                ViewBag.Code = productcode;
                return View(list);
            }

            skip = (page - 1) * countproduct;


            totalcount = _admin.CountProducts();
            double remainall = totalcount % countproduct;
            ViewBag.pageId = page;
            if (remainall == 0)
            {
                ViewBag.PageCount = totalcount / countproduct;
            }
            else
            {
                ViewBag.PageCount = (totalcount / countproduct) + 1;
            }

            var listproduct = await _admin.GetProducts(skip, countproduct);
            return View(listproduct);
        }


        #region Amazing
        [Route("/Admin/AmazingProducts")]
        public async Task<IActionResult> Amazing(string productcode, string productname, int page = 1)
        {
            int countproduct = 20;
            int totalcount = 0;
            int skip = (page - 1) * countproduct;
            if (productcode != null || productname != null)
            {
                totalcount = _admin.CountAmazingProductsBySearch(productcode, productname);
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
                ViewBag.Code = productcode;
                var list = await _admin.SearchAmazingProducts(productcode, productname, skip, countproduct);
                ViewBag.Name = productname;
                ViewBag.Code = productcode;
                return View(list);
            }

            skip = (page - 1) * countproduct;


            totalcount = _admin.CountAmazingProducts();
            double remainall = totalcount % countproduct;
            ViewBag.pageId = page;
            if (remainall == 0)
            {
                ViewBag.PageCount = totalcount / countproduct;
            }
            else
            {
                ViewBag.PageCount = (totalcount / countproduct) + 1;
            }

            var listproduct = await _admin.GetAmazingProducts(skip, countproduct);
            return View(listproduct);
        }
        #endregion

        #region UnShow 
        [Route("/Admin/UnShowProducts")]
        public async Task<IActionResult> UnShowProduct(string productcode, string productname, int page = 1)
        {
            int countproduct = 20;
            int totalcount = 0;
            int skip = (page - 1) * countproduct;
            if (productcode != null || productname != null)
            {
                totalcount = _admin.CountUnShowProductsBySearch(productcode, productname);
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
                ViewBag.Code = productcode;
                var list = await _admin.SearchUnShowProducts(productcode, productname, skip, countproduct);
                ViewBag.Name = productname;
                ViewBag.Code = productcode;
                return View(list);
            }

            skip = (page - 1) * countproduct;


            totalcount = _admin.CountUnShowProducts();
            double remainall = totalcount % countproduct;
            ViewBag.pageId = page;
            if (remainall == 0)
            {
                ViewBag.PageCount = totalcount / countproduct;
            }
            else
            {
                ViewBag.PageCount = (totalcount / countproduct) + 1;
            }

            var listproduct = await _admin.GetUnShowProducts(skip, countproduct);
            return View(listproduct);
        }
        #endregion



        public async Task<IActionResult> AddOrEditProduct(Guid id)
        {
            var categories = await _admin.GetGategories();
            var colors = await _admin.GetColors();

            if (id == Guid.Empty)
            {
                ViewBag.Category = new SelectList(categories.Where(c => c.CategoryLevel == 2), "Id", "CategoryName");
                ViewBag.Color = new SelectList(colors, "ColorCode", "ColorName");
                return View(new ProductViewModel());
            }

            else
            {
                var product = await _admin.GetProductById(id);
                if (product == null)
                {
                    return NotFound();
                }


                string[] productcolors = null;
                if (product.Color != null)
                {
                    List<string> color = product.Color.Split(',').ToList();
                    productcolors = color.ToArray();
                }

                ProductViewModel model = new ProductViewModel
                {

                    CountProduct = product.CountProduct,
                    DesProduct = product.ProductDetail.Description,
                    Feature = product.ProductDetail.Feature,
                    Id = id,
                    ProductImgName = product.ProductImg,
                    MetaDescription = product.ProductDetail.MetaDescription,
                    MetaTag = product.ProductDetail.MetaTag,
                    Off = product.Off,
                    Price = product.Price,
                    ProductName = product.ProductName,
                    Color = productcolors,
                    Amazing = product.Amazing
                };
                ViewBag.Category = new SelectList(categories.Where(c => c.CategoryLevel == 2), "Id", "CategoryName", product.CategoryId);
                ViewBag.Color = new SelectList(colors, "ColorCode", "ColorName");
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddAndUpdateProduct(ProductViewModel model)
        {
            if (model.ProductName == null)
            {
                return Json(0);
            }

            if (model.DesProduct == null)
            {
                return Json(1);
            }
            if (model.Feature == null)
            {
                return Json(2);
            }


            if (model.Id == Guid.Empty)
            {
                if (model.ImageProduct == null)
                {
                    return Json(3);
                }
            }


            if (model.ImageProduct != null)
            {

                string strExt = Path.GetExtension(model.ImageProduct.FileName);

                if (strExt != ".jpg" && strExt != ".jpeg" && strExt != ".png")
                {
                    return Json(4);
                }
                else if (!ImageValidator.IsImage(model.ImageProduct)) //چک کردن تصویر
                {
                    return Json(5);
                }

            }


            if (model.Id == Guid.Empty)//این قسمت برای ثبت محصول
            {
                if (_admin.ExistNameCategoryForAdd(model.ProductName))
                {
                    return Json(6);
                }

                if (model.ImagesGallery != null)
                {

                    if (model.ImagesGallery.Count() < 4)
                    {
                        return Json(7);
                    }


                    foreach (var file in model.ImagesGallery)
                    {
                        string strExt = Path.GetExtension(file.FileName);

                        if (strExt != ".jpg" && strExt != ".jpeg" && strExt != ".png")
                        {
                            return Json(4);
                        }
                        else if (!ImageValidator.IsImage(file)) //چک کردن تصویر
                        {
                            return Json(5);
                        }

                    }
                }
                _admin.AddProduct(model);
                return Json(new { redirectToUrl = Url.Action("Index", "Product") });
            }
            else //ویرایش محصول
            {
                if (_admin.ExistNameCategoryForUpdate(model.ProductName, model.Id))
                {
                    return Json(6);
                }
                _admin.UpdateProduct(model);
                return Json(new { redirectToUrl = Url.Action("Index", "Product") });
            }

        }


        public async Task<IActionResult> ShowAndEditGallery(Guid id)
        {

            ProductGalleryViewModel model = new ProductGalleryViewModel();
            var Images_Gallery = await _admin.GetLsitProductGallery(id);
            if (Images_Gallery.Count() > 0)
            {
                model.GalleryImagesName = Images_Gallery;
                model.Id = id;
                return View(model);
            }
            else
            {
                model.Id = id;
                return View(model);
            }



        }
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> UpdateImageGallery(UpdateImageViewModel model)
        {
            if (model != null)
            {
                string strExt = Path.GetExtension(model.SingleImageProduct.FileName);

                if (strExt != ".jpg" && strExt != ".jpeg" && strExt != ".png")
                {
                    return Json(1);
                }
                else if (!ImageValidator.IsImage(model.SingleImageProduct)) //چک کردن تصویر
                {
                    return Json(2);
                }
                else
                {
                    var Image_Gallery = await _admin.GetImageGalleryById(model.Id);
                    if (Image_Gallery == null)
                    {
                        return Json(3);
                    }
                    else
                    {
                        _admin.UpdateOneImageGallery(model);
                        return Json(445);
                    }


                }
            }
            return Json(0);

        }


        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> UpdateImageProduct(UpdateImageViewModel model)
        {
            if (model != null)
            {
                string strExt = Path.GetExtension(model.SingleImageProduct.FileName);

                if (strExt != ".jpg" && strExt != ".jpeg" && strExt != ".png")
                {
                    return Json(1);
                }
                else if (!ImageValidator.IsImage(model.SingleImageProduct)) //چک کردن تصویر
                {
                    return Json(2);
                }
                else
                {
                    var Image_Gallery = await _admin.GetProductById(model.Id);
                    if (Image_Gallery == null)
                    {
                        return Json(3);
                    }
                    else
                    {
                        _admin.UpdateMainImageProduct(model);
                        return Json(new { redirectToUrl = Url.Action("Index", "Product") });
                    }


                }
            }
            return Json(0);

        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> AddGalleryProduct(ProductGalleryViewModel model)
        {

            if (model.GalleryImages == null || model.GalleryImages.Count() < 4 || model.GalleryImages.Count() > 4)
            {
                return Json(1);
            }

            foreach (var file in model.GalleryImages)
            {
                string strExt = Path.GetExtension(file.FileName);

                if (strExt != ".jpg" && strExt != ".jpeg" && strExt != ".png")
                {
                    return Json(2);
                }
                else if (!ImageValidator.IsImage(file)) //چک کردن تصویر
                {
                    return Json(3);
                }

            }


            var ProductForGallery = await _admin.GetProductById(model.Id);
            if (ProductForGallery == null)
            {
                return Json(4);
            }
            else
            {
                _admin.AddGalleryListForProduct(model);
                return Json(12316);
            }


        }
        public async Task<IActionResult> ShowDetail(Guid id)
        {
            var product = await _admin.GetProductById(id);
            string Full_Cat = await _admin.GetFullCategory(product.CategoryId);
            string Full_ColorName = "";
            string Date_Created = product.ProductDetail.DateProduct + "-" + product.ProductDetail.TimeProduct;
            if (product.Color != null)
            {
                Full_ColorName = await _admin.GetFullColorNamesForProductDetail(product.Color);
            }


            DetailProductViewModel model = new DetailProductViewModel()
            {
                Amazing = product.Amazing,
                Category = Full_Cat,
                Color = Full_ColorName,
                CountProduct = product.CountProduct,
                IsShowProduct = product.IsShowProduct,
                Off = product.Off,
                Price = product.Price,
                FinalPrice = product.FinalPrice,
                ProductCode = product.ProductCode,
                ProductImgName = product.ProductImg,
                ProductName = product.ProductName,
                DesProduct = product.ProductDetail.Description,
                Feature = product.ProductDetail.Feature,
                MetaTag = product.ProductDetail.MetaTag,
                MetaDescription = product.ProductDetail.MetaDescription,
                DateAndTime = Date_Created,


            };


            return View(model);
        }


        public IActionResult AmazingProduct(Guid id)
        {
            _admin.Amazing(id);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult ShowProduct(Guid id)
        {
            bool IsActon = _admin.ShowProduct(id);
            if (IsActon == true)
            {
                return RedirectToAction("UnShowProduct");
            }


            return RedirectToAction(nameof(Index));
        }
        public IActionResult Remove(Guid id)
        {
            _admin.RomoveProduct(id);
            return RedirectToAction(nameof(Index));

        }



    }
}
