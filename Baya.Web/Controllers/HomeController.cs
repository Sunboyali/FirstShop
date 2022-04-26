using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Baya.Core.InterFaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Baya.Core.ViewModels;
using Microsoft.AspNetCore.Diagnostics;

namespace Baya.Web.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
        private readonly IProduct _product;
       

        public HomeController(IProduct product)
        {
            //_logger = logger;
            _product = product;
        }
        public IActionResult Index()
        {
            return View();
        }


       

        [HttpPost]
        public async Task<IActionResult> SearchResult(string txtsearch)
        {
            SearchListModelViewModel ListSearch = new SearchListModelViewModel();
            ListSearch.ProductListSearch = await _product.SearchProductByText(txtsearch);
            ListSearch.ProductListSearchByCategory = await _product.SearchProductByCategory(txtsearch);
            return PartialView("_SearchResult", ListSearch);
        }







        //public IActionResult Privacy()
        //{
        //    return View();
        //}

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
