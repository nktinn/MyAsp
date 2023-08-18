using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyAsp.Models;

namespace MyAsp.Controllers
{
    public class HomeController : Controller
    {
        [Route("Home/Error/{statusCode}")]
        public IActionResult Error(int? statusCode)
        {
            if (statusCode.HasValue && statusCode.Value == 404)
            {
                return View("NotFound");
            }
            return View();
        }

    }
}