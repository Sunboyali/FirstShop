using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Baya.Core.ViewModels;
using System.Security.Claims;
using Baya.Core.InterFaces;
using Baya.Core.Classes;

namespace Baya.Web.Controllers
{
    public class PaymentController : Controller
    {

        private readonly IPayment _payment;

        public PaymentController(IPayment payment)
        {
            _payment = payment;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Payment()
        {
            string CurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            string orderNumber = CodeGenerators.FactorCode();
            bool checkFactor = _payment.UpdateFactor(Guid.Parse(CurrentUserId), orderNumber);

            if (!checkFactor)
            {
                _payment.AddFactor(Guid.Parse(CurrentUserId), orderNumber);
            }

            Guid factorID = _payment.GetFactorById(orderNumber);
            var payment = new ZarinpalSandbox.Payment(Convert.ToInt32(_payment.GetTotalPriceFactor(factorID)));
            var result = payment.PaymentRequest("تراکنش جدید", "https://localhost:44378/Payment/PaymentCallBack/" + factorID);


            if (result.Result.Status == 100)
            {
                return Redirect("https://sandbox.zarinpal.com/pg/StartPay/" + result.Result.Authority);
            }


            return Redirect("/Payment/ResultPayment/" + factorID);
        }

        public async Task<IActionResult> PaymentCallBack(Guid id)
        {
            var factor = await _payment.GetFactor(id);
            string authority = HttpContext.Request.Query["Authority"];

            var payment = new ZarinpalSandbox.Payment(Convert.ToInt32(factor.TotalPrice));
            var result = payment.Verification(authority).Result;

            if (result.Status == 100)
            {
                _payment.UpdatePayment(id, "فاکتو فروش زرین پال", "زرین پال", result.RefId.ToString(), result.RefId.ToString());
            }

            return Redirect("/Payment/ResultPayment/" + id);
        }
        public async Task<IActionResult> ResultPayment(Guid id)
        {
            var result = await _payment.GetFactor(id);

            return View(result);
        }

    }
}
