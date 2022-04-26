using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Baya.Core.InterFaces;
using Baya.Core.ViewModels;
using System.IO;
using Baya.Core.Classes;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Baya.Web.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    //[RoleAttribute("Admin")]
    public class SliderController : Controller
    {

        private readonly IAdmin _admin;


        public SliderController(IAdmin admin)
        {
            _admin = admin;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _admin.GetSliders();
            return View(list);
        }





        public IActionResult Activate(Guid id)
        {
            _admin.ActivateSlider(id);
            return RedirectToAction(nameof(Index));
        }


        [Route("/Admin/SlideImages/{id}")]
        public async Task<IActionResult> SlideImages(Guid id)
        {


            ViewBag.SliderName = _admin.GetSliderName(id);
            ViewBag.Id = id;

            var slideimages = await _admin.GetSliderImages(id);


            return View(slideimages);
        }


        public async Task<IActionResult> AddOrEditSlide(Guid Id,Guid SliderId)
        {
            if (Id == Guid.Empty)
            {
                SlideImageViewModel model = new SlideImageViewModel()
                {
                    SlideId = SliderId
                };

                return View(model);
            }
            else
            {
                var sliderimage = await _admin.GetSliderImageById(Id);
                var listimages = await _admin.GetSliderImages(SliderId);
                
                SlideImageViewModel imgslider = new SlideImageViewModel()
                {
                    Id = sliderimage.Id,
                    AltImage = sliderimage.AltImage,
                    ImageName = sliderimage.ImageName,
                    Link = sliderimage.Link,
                    SlideId = SliderId,
                    TitleImage = sliderimage.TitleImage,
                    DisplaySlide = sliderimage.DisplaySlide

                };
                ViewBag.NameSlider = sliderimage.Slider.Name;
                ViewBag.ListLevel = new SelectList(listimages, "DisplaySlide", "DisplaySlide",sliderimage.DisplaySlide);

                return View(imgslider);
            }
         
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddAndUpdateSlideImg(SlideImageViewModel model)
        {         


            if (model.Id == Guid.Empty)
            {
                if (model.ImageSlide == null)
                {
                    return Json(0);
                }
            }

            if (model.ImageSlide != null)
            {

                string strExt = Path.GetExtension(model.ImageSlide.FileName);

                if (strExt != ".jpg" && strExt != ".jpeg" && strExt != ".png")
                {
                    return Json(1);
                }
                else if (!ImageValidator.IsImage(model.ImageSlide)) //چک کردن تصویر
                {
                    return Json(2);
                }

            }
            _admin.AddAndUpdateSlideImage(model);
            return Json(new { redirectToUrl = Url.Action("SlideImages", "Slider", new { Id= model.SlideId }) });
        }



        public IActionResult UpdateNumberSlide(Guid Id,int numberslide)
        {
            _admin.UpdateNumberSlide(Id, numberslide);
            return Json(100);
        }











        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Route("/Admin/AddSlideImage/{Id}")]
        //public IActionResult AddSlide(Guid Id, AddSlideImageViewModel viewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (_admin.IsNumberSlide(Id,viewModel.DisplaySlide))
        //        {
        //            ViewBag.ErrorThree = true;
        //            ViewBag.Id = Id;
        //            return View(viewModel);
        //        }




        //        int imageslide = _admin.AddSlideImage(Id, viewModel);
        //        if (imageslide == 1)
        //        {
        //            ViewBag.ErrorOne = true;

        //        }
        //        else if (imageslide == 2)
        //        {
        //            ViewBag.ErrorThree = true;

        //        }
        //        else
        //        {
        //            return RedirectToAction("SlideImages", new { Id = Id });
        //        }
        //    }





        //    ViewBag.Id = Id;


        //    return View(viewModel);
        //}


        //[Route("/Admin/EditSlideImage/{id}")]
        //public async Task<IActionResult> EditSlide(Guid Id)
        //{
        //    var slider = await _admin.GetSliderImageById(Id);

        //    UpdateSlideImageViewModel model = new UpdateSlideImageViewModel()
        //    {
        //        AltImage = slider.AltImage,
        //        DisplaySlide = slider.DisplaySlide,
        //        ImageName = slider.ImageName,
        //        Link = slider.Link,
        //        TitleImage = slider.TitleImage,
        //    };

        //    ViewBag.Id = slider.SliderId;

        //    ViewBag.NameSlider = slider.Slider.Name;
        //    ViewBag.Image = slider.ImageName;
        //    ViewBag.ErrorOne = false;
        //    ViewBag.ErrorTwo = false;
        //    ViewBag.ErrorThree = false;

        //    return View(model);
        //}
        //[Route("/Admin/EditSlideImage/{id}")]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> EditSlide(Guid Id,UpdateSlideImageViewModel viewModel)
        //{
        //    var slider = await _admin.GetSliderImageById(Id);
        //    if (ModelState.IsValid)
        //    {
        //        int imageslide = _admin.UpdateImageSlider(Id, viewModel);
        //        if (imageslide == 1)
        //        {
        //            ViewBag.ErrorOne = true;

        //        }
        //        else if (imageslide == 2)
        //        {
        //            ViewBag.ErrorThree = true;

        //        }
        //        else
        //        {
        //            ViewBag.Id = slider.SliderId;
        //            return RedirectToAction("SlideImages", new { Id = ViewBag.Id });
        //        }
        //    }
        //    ViewBag.NameSlider = slider.Slider.Name;
        //    ViewBag.Image = slider.ImageName;
        //    ViewBag.Id = slider.SliderId;
        //    ViewBag.Image = slider.ImageName;
        //    return View(viewModel);

        //}

        public async Task<IActionResult> DeleteImageSlide(Guid id)
        {


            var slider = await _admin.GetSliderImageById(id);

            ViewBag.Id = slider.SliderId;
            _admin.DeleteSlideImage(id);



            return RedirectToAction("SlideImages", new { Id = ViewBag.Id });
        }



    }
}
