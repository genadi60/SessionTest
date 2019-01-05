using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SessionTest.InputModels;
using SessionTest.ViewModels;

namespace SessionTest.Controllers
{
    public class ShippingController : BaseController
    {
        public ShippingController()
        {
            
        }

        public IActionResult Index(string id)
        {
            var inputModel = new ShippingDataInputModel
            {
                CartId = id
            };
            return View(inputModel);
        }

        [HttpPost]
        public IActionResult Create(ShippingDataInputModel model)
        {
            return RedirectToAction("Index", "Package", model);
        }
    }
}
