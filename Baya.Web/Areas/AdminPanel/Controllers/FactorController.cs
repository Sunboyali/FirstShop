using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Baya.Core.InterFaces;
using Microsoft.AspNetCore.Mvc;

namespace Baya.Web.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    //[RoleAttribute("Admin")]
    public class FactorController : Controller
    {
        private readonly IAdmin _admin;

        public FactorController(IAdmin admin)
        {
            _admin = admin;
        }

        [Route("/Admin/SuccessFactors")]
        public async Task<IActionResult> SuccessFactors(string search_mobile,string fromDate,string todate, int page = 1)
        {


            if (fromDate!= null || todate != null)
            {
                int totalcount = 0;
                if (fromDate != null)
                {
                    if (todate != null)
                    {
                        //FromDate And ToDate
                        totalcount = _admin.CountFactorByTwoDate(fromDate, todate);
                    }
                    else
                    {
                        //FromDate
                        totalcount = _admin.CountFactorByFromDate(fromDate);
                    }
                }

                if (fromDate == null && todate != null)
                {
                    //ToDate
                    totalcount = _admin.CountFactorByToDate(todate);
                }


             
                int countfactor = 20;
                int skip = (page - 1) * countfactor;
               
                double remain = totalcount % countfactor;

                ViewBag.pageId = page;
                if (remain == 0)
                {
                    ViewBag.PageCount = totalcount / countfactor;
                }
                else
                {
                    ViewBag.PageCount = (totalcount / countfactor) + 1;
                }


                if (fromDate != null)
                {
                    if (todate != null)
                    {

                        ViewBag.FromDate = fromDate;
                        ViewBag.ToDate = todate;

                        //FromDate And ToDate
                        var list =await  _admin.GetFactorsFromAndToDate(fromDate, todate, skip, countfactor);

                        return View(list);

                    }
                    else
                    {
                        ViewBag.FromDate = fromDate;
                        //FromDate
                        var list = await _admin.GetFactorsFromDate(fromDate,  skip, countfactor);
                        return View(list);
                    }
                }

               else 
                {
                    ViewBag.ToDate = todate;
                    //ToDate
                    var list = await _admin.GetFactorsToDate(todate, skip, countfactor);
                    return View(list);
                }







                //var list = await _admin.GetSuccessFactors(search_mobile, skip, countfactor);

                //return View(list);
            }
           else if (search_mobile != null)
            {
                int countfactor = 20;
                int skip = (page - 1) * countfactor;
                int totalcount = _admin.CountFactorWithMobile(search_mobile);
                double remain = totalcount % countfactor;

                ViewBag.pageId = page;
                if (remain == 0)
                {
                    ViewBag.PageCount = totalcount / countfactor;
                }
                else
                {
                    ViewBag.PageCount = (totalcount / countfactor) + 1;
                }


                ViewBag.searchString = search_mobile;
                var list = await _admin.GetSuccessFactors(search_mobile, skip, countfactor);
                

                return View(list);
            }
            else
            {
                int countfactor = 20;
                int skip = (page - 1) * countfactor;
                int totalcount = _admin.CountFactor();
                double remain = totalcount % countfactor;

                ViewBag.pageId = page;
                if (remain == 0)
                {
                    ViewBag.PageCount = totalcount / countfactor;
                }
                else
                {
                    ViewBag.PageCount = (totalcount / countfactor) + 1;
                }



                var list = await _admin.GetSuccessFactors(search_mobile, skip, countfactor);

                return View(list);
            }

            
          
          
        }

        [Route("/Admin/Detail/{id}")]
        public async Task<IActionResult> DetailFactor(Guid id)
        {
            var listdetail = await _admin.GetDetailFactor(id);
            return View(listdetail);
        }
    }
}
