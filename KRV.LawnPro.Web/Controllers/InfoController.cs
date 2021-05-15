using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KRV.LawnPro.Web.Controllers
{
    public class InfoController : Controller
    {
        public IActionResult ContactUs()
        {
            ViewBag.Title = "Contact Information";
            return View();
        }


        public IActionResult OurServices()
        {
            ViewBag.Title = "Our Services";
            return View();
        }


    }
}
