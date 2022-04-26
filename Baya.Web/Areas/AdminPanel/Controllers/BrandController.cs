using Baya.Core.InterFaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Baya.DataLayer.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Baya.Web.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    //[RoleAttribute("Admin")]
    public class BrandController : Controller
    {
        private readonly IAdmin _admin;

        public BrandController(IAdmin admin)
        {
            _admin = admin;
        }

        public async Task<IActionResult> Index()
        {
            var brands = await _admin.GetBrands();
            return View(brands);
        }


        public async Task<IActionResult> AddOrEdit(Guid id)
        {
            var categories = await _admin.GetGategories();
            ViewBag.MainCategory = new SelectList(categories.Where(c => c.ParentId == null), "Id", "CategoryName");
            if (id == Guid.Empty)
           
            return View(new Brand());
            else
            {
                var brandModel = await _admin.GetBrandById(id);
                if (brandModel == null)
                {
                    return NotFound();
                }
                return View(brandModel);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(Guid id,Brand brand)
        {
            
            if (id == Guid.Empty)
            {
                _admin.AddBrand(brand);
                return RedirectToAction("Index");
            }
            else
            {
                if (_admin.ExistBrandId(id))
                {
                    try
                    {
                        _admin.UpdateBrand(brand);
                        return RedirectToAction("Index");
                    }
                    catch
                    {

                        throw;
                    }
                }
                return NotFound();
              
              
            }
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> AddOrEdit(Guid id)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        //Insert
        //        if (id == 0)
        //        {
        //            transactionModel.Date = DateTime.Now;
        //            _context.Add(transactionModel);
        //            await _context.SaveChangesAsync();

        //        }
        //        //Update
        //        else
        //        {
        //            try
        //            {
        //                _context.Update(transactionModel);
        //                await _context.SaveChangesAsync();
        //            }
        //            catch (DbUpdateConcurrencyException)
        //            {
        //                if (!TransactionModelExists(transactionModel.TransactionId))
        //                { return NotFound(); }
        //                else
        //                { throw; }
        //            }
        //        }
        //        return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", _context.Transactions.ToList()) });
        //    }
        //    return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit", transactionModel) });
        //}



    }
}
