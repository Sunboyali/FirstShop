using Baya.Core.InterFaces;
using Baya.Core.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Baya.Web.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    //[RoleAttribute("Admin")]
    public class CityController : Controller
    {
        private readonly IAdmin _admin;

        public CityController(IAdmin admin)
        {
            _admin = admin;
        }

        public async Task<IActionResult> Index()
        {
            var states = await _admin.GetStates();
            return View(states);
        }


        public async Task<IActionResult> AddOrEditState(Guid Id)
        {


            if (Id == Guid.Empty)           
            return View(new CityViewModel());

            //

            var state = await _admin.GetStateById(Id);
            CityViewModel model = new CityViewModel()
            {
                Id = state.Id,
                Name = state.Name,
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddAndUpdateState(CityViewModel model)
        {
            if (_admin.ExistStateName(model))
            {
                return Json(0);
            }
            _admin.AddAndUpdateState(model);
            return Json(new { redirectToUrl = Url.Action("Index", "City") });
        }


        public IActionResult ImportExcelState()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ImportExcel(IFormFile ExcelFile)
        {
            using (var stream = new MemoryStream())
            {
                await ExcelFile.CopyToAsync(stream);

                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    for (int i = 2; i <= rowCount; i++)
                    {
                        CityViewModel viewModel = new CityViewModel()
                        {
                            Name = worksheet.Cells[i, 1].Value.ToString().Trim()
                        };
                        if (viewModel.Name != null)
                        {
                            if (!_admin.ExistStateNameExcel(viewModel.Name))
                            {
                                _admin.AddExcelState(viewModel);
                            }
                        }
                       
                       
                    }
                }
            }

            return RedirectToAction(nameof(Index));
        }


        public IActionResult DeleteState(Guid Id)
        {
            _admin.RemoveState(Id);
            return RedirectToAction(nameof(Index));
        }
      

        #region City
        public async Task<IActionResult> Cities(Guid Id)
        {
            var states = await _admin.GetCities(Id);

            ViewBag.Id = Id;
            return View(states);
        }


        public async Task<IActionResult> AddOrEditCity(Guid Id,Guid StateId)
        {

            if (Id == Guid.Empty)
            {
                CityViewModel model = new CityViewModel()
                {
                    StateId = StateId,
                };


                return View(model);
            }


            //
            else
            {
                var state = await _admin.GetStateById(Id);
                var states = await _admin.GetStates();
                ViewBag.States = new SelectList(states, "Id", "Name", StateId);
                CityViewModel model = new CityViewModel()
                {
                    Id = state.Id,
                    Name = state.Name,
                    StateId = state.StateId
                };
                return View(model);
            }
          
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddAndUpdateCity(CityViewModel model)
        {
            if (_admin.ExistCityName(model))
            {
                return Json(0);
            }
            _admin.AddAndUpdateCity(model);
            return Json(new { redirectToUrl = Url.Action("Cities", "City", new { Id = model.StateId }) });
        }
        public IActionResult ImportExcelCity(Guid StateId)
        {
            ViewBag.Id = StateId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ImportExcelCities(Guid Id, IFormFile ExcelFile)
        {
            using (var stream = new MemoryStream())
            {
                await ExcelFile.CopyToAsync(stream);

                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    for (int i = 2; i <= rowCount; i++)
                    {
                        CityViewModel viewModel = new CityViewModel()
                        {
                            Name = worksheet.Cells[i, 1].Value.ToString().Trim(),
                            StateId =Id
                        };

                        if (viewModel.Name != null)
                        {
                            if (!_admin.ExistCityNameExcel(Id, viewModel.Name))
                            {
                                _admin.AddExcelCity(viewModel);
                            }
                        }
                        

                    }
                }
            }

            return RedirectToAction("Cities", "City", new { Id = Id });
        }
        public IActionResult DeleteCity(Guid Id)
        {
            Guid? StateId = _admin.RemoveCity(Id);

            return RedirectToAction("Cities", "City", new { Id = StateId });
        }

        #endregion


    }
}
