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

        [Authorize]
        public IActionResult Index(string id)
        {
            var inputModel = new ShippingDataInputModel
            {
                CartId = id
            };
            return View(inputModel);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(ShippingDataInputModel model)
        {
            ViewBag.ShippingData = model;

            return RedirectToAction("Index", "Payment", new{id = model.CartId});
        }
    }
}
