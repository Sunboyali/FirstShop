using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Baya.Core.ViewModels;
using Baya.Core.InterFaces;
using Baya.DataLayer.Entities;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using Baya.Core.Classes;

namespace Baya.Web.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    //[RoleAttribute("Admin")]
    public class CategoryController : Controller
    {
        private readonly IAdmin _admin;
        public CategoryController(IAdmin admin)
        {
            _admin = admin;
        }

        [Route("/Admin/Categories")]
        public async Task<IActionResult> Index()
        {

            var categories = await _admin.GetGategories();

            //List<CategoryTreeViewNode> nodes = new List<CategoryTreeViewNode>();
            //nodes.Add(new TreeViewNode
            //{
            //    id = "asd",
            //    parent = "#",
            //    text = "دسته بندی های سایت"
            //});


            //Loop and add the Parent Nodes.
            //foreach (Category type in categories.Where(c => c.ParentId == null))
            //{
            //    nodes.Add(new CategoryTreeViewNode { id = type.Id.ToString(), parent = "#", text = type.CategoryName });
            //}

            ////Loop and add the Child Nodes.
            //foreach (var item in categories)
            //{
            //    nodes.Add(new CategoryTreeViewNode
            //    {
            //        id = item.Id.ToString(),
            //        text = item.CategoryName,
            //        parent = item.ParentId.ToString()

            //    });
            //}

            //ViewBag.Json = JsonConvert.SerializeObject(nodes);
            //ViewBag.ViewTitle = "نمایش دسته بندی سایت";

            return View(categories);
        }





        public async Task<IActionResult>AddAndEditMainCategory(Guid id)
        {
            if (id == Guid.Empty)
                return View(new MainCategoryViewModel());
            //else
            var Model = await _admin.GetCategoryById(id);
            if (Model == null)
                return NotFound();
            MainCategoryViewModel viewModel = new MainCategoryViewModel()
            {
                CategoryImageName = Model.CategoryImg,
                CategoryName = Model.CategoryName,
                Id = id

            };
            return View(viewModel);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddAndUpdateMain(MainCategoryViewModel model)
        {
            if (model.CategoryName == null)            
                return Json(0);
            

            if (model.Id == Guid.Empty)
            {
                if (model.MainCatImg == null)
                    return Json(1);
                if (_admin.ExistNameCategoryForAdd(model.CategoryName))
                    return Json(4);
            }
            else
            {
                if (_admin.ExistNameCategoryForUpdate(model.CategoryName, model.Id))
                    return Json(4);
                if (!_admin.ExistCategory(model.Id))                
                return Json(5);
            }



            if (model.MainCatImg != null)
            {
                string strExt = Path.GetExtension(model.MainCatImg.FileName);

                if (strExt != ".jpg" && strExt != ".jpeg" && strExt != ".png")                
                    return Json(2);
                
                else if (!ImageValidator.IsImage(model.MainCatImg)) //چک کردن تصویر                
                    return Json(3);             

            }
           

            _admin.AddOrEditMainCategory(model);
            return Json(new { redirectToUrl = Url.Action("Index", "Category") });

          
        }


        public async Task<IActionResult> AddAndEditFirstSub(Guid id, Guid parentid)
        {
            if (parentid == Guid.Empty)
                return View(new SubCategoryViewModel());

            else//Update
            {
                var categories = await _admin.GetGategories();
                ViewBag.MainCategory = new SelectList(categories.Where(c => c.ParentId == null), "Id", "CategoryName");
                var Model = await _admin.GetCategoryById(id);
                if (Model == null)
                {
                    return NotFound();
                }
                SubCategoryViewModel viewModel = new SubCategoryViewModel()
                {
                    CategoryName = Model.CategoryName,
                    Id = id,
                    ParentId = Model.ParentId

                };
                return View(viewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddAndUpdateFirstSub(SubCategoryViewModel model)
        {

            if (model.CategoryName == null)
            {
                return Json(0);
            }


            if (model.ParentId == null)
            {
                if (_admin.ExistNameCategoryForAdd(model.CategoryName))
                {
                    return Json(1);
                }
               
              
            }
            else
            {
                if (_admin.ExistNameCategoryForUpdate(model.CategoryName, model.Id))
                {
                    return Json(1);
                }
                
               
            }

            _admin.AddOrUpdateSubCategory(model);
            return Json(new { redirectToUrl = Url.Action("Index", "Category") });
        }



        public async Task<IActionResult> AddOrEditSecondSub(Guid id, Guid mainid, Guid firstid, byte level)
        {
            ViewBag.Level = level;
            if (level == 1)
                return View(new SubCategoryViewModel());

            else//Update
            {

                var categories = await _admin.GetGategories();
                ViewBag.MainCategory = new SelectList(categories.Where(c => c.ParentId == null), "Id", "CategoryName", mainid);
                ViewBag.FirstSubCategory = new SelectList(categories.Where(c => c.ParentId == mainid), "Id", "CategoryName", firstid);
                var Model = await _admin.GetCategoryById(id);
                if (Model == null)
                {
                    return NotFound();
                }
                SubCategoryViewModel viewModel = new SubCategoryViewModel()
                {
                    CategoryName = Model.CategoryName,
                    Id = id,
                    ParentId = Model.ParentId,
                    Level = level

                };
                return View(viewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddAndUpdateSecondSub(SubCategoryViewModel model)
        {

            if (model.CategoryName == null)
            {
                return Json(0);
            }


            if (model.Level == 1)
            {
                if (_admin.ExistNameCategoryForAdd(model.CategoryName))
                {
                    return Json(1);
                }
                
            }
            else
            {
                if (_admin.ExistNameCategoryForUpdate(model.CategoryName, model.Id))
                {
                    return Json(1);
                }

                if (model.ParentId == null)
                {
                    return Json(2);
                }

                
            }
            _admin.AddOrUpdateSecondSubCategory(model);
            return Json(new { redirectToUrl = Url.Action("Index", "Category") });
        }

        public async Task<IActionResult> GetSubCategory(Guid id)
        {
            var SubCategory_List = await _admin.GetGategories();           
            return Json(new SelectList(SubCategory_List.Where(c => c.ParentId == id), "Id", "CategoryName"));
        }    
        

        public IActionResult Delete(Guid id)
        {
            _admin.DeleteCategory(id);
            return RedirectToAction(nameof(Index));
        }


    }
}
