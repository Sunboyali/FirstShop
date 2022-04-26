using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Baya.Core.InterFaces;
using Microsoft.AspNetCore.Mvc;
using Baya.Core.ViewModels;
using Baya.Core.Classes;
using System.Globalization;

namespace Baya.Web.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    //[RoleAttribute("Admin")]
    public class PollController : Controller
    {
        private IAdmin _admin;

        public PollController(IAdmin admin)
        {
            _admin = admin;
        }
        [Route("/Admin/Polls")]
        public async Task<IActionResult> Index(int page = 1)
        {
            int count = 20;
            int skip = (page - 1) * count;
            int totalcount = _admin.CountPolls();
            double remain = totalcount % count;
            ViewBag.pageId = page;
            if (remain == 0)
            {
                ViewBag.PageCount = totalcount / count;
            }
            else
            {
                ViewBag.PageCount = (totalcount / count) + 1;
            }

            var list = await _admin.GetPolls(skip, count);

            return View(list);
        }



        public async Task<IActionResult> AddOrEditPoll(Guid Id)
        {
            ViewBag.MyDate = DateConvertor.ToShamsi();
            if (Id == Guid.Empty)
                return View(new PollViewModel());

            //else
            var poll = await _admin.GetPollById(Id);



            PollViewModel model = new PollViewModel();
            model.AnswerListEdit = await _admin.GetQuestionsListByPollId(Id);
            model.Id = Id;
            model.Question = poll.Question;
            model.StartDate = poll.StartDate;
            model.EndDate = poll.EndDate;

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddAndUpdatePoll(PollViewModel model)
        {
            
            if (model.Question == null)
            {
                return Json(0);
            }
            else if (model.StartDate == null)
            {
                return Json(1);
            }
            else if (model.EndDate == null)
            {
                return Json(2);
            }
            else if (model.Id == Guid.Empty)
            {
                if (model.Answer == null)
                {
                    return Json(3);
                }
                model.AnswerList = model.Answer.Split(new string[] { Environment.NewLine },
                  StringSplitOptions.RemoveEmptyEntries);
                if (model.AnswerList.Count() > 8 || model.AnswerList.Count() < 2)
                {
                    return Json(4);
                }
            }
            if (DateTime.Parse(model.StartDate, CultureInfo.InvariantCulture) > DateTime.Parse(model.EndDate, CultureInfo.InvariantCulture))
            {
                return Json(5);
            }


            _admin.AddAndUpdatePoll(model);
            return Json(new { redirectToUrl = Url.Action("Index", "Poll") });

        }

        //[HttpPost]
        public async Task<IActionResult> UpdatePollOption(Guid Id, string polloptiontext)
        {
            if (polloptiontext == null)
            {
                return Json(1);
            }


            var polloption = await _admin.GetPollOptionById(Id);
            if (polloption != null)
            {
                _admin.UpdatePollOption(Id, polloptiontext);
                return Json(2);
            }
            return Json(0);
        }


        public async Task<IActionResult> ShowResult(Guid Id)
        {
            var charts = new List<ChartViewModel>();
            var Getresalt = await _admin.GetQuestionsListByPollId(Id);

            foreach (var polloption in Getresalt)
            {
                ChartViewModel chart = new ChartViewModel()
                {
                    Color = ColorGenerators.SelectColorCode(),
                    Label = polloption.Answer,
                    Value = polloption.VoteCount
                };
                charts.Add(chart);
            }
            ViewBag.Vots = _admin.SumVotes(Id);

            return View(charts);
        }
        public IActionResult ActivatePoll(Guid Id)
        {
            _admin.ActivatePoll(Id);
            return RedirectToAction(nameof(Index));

        }
        public IActionResult Delete(Guid id)
        {
            _admin.DeletePoll(id);
            return RedirectToAction(nameof(Index));

        }



    }
}
