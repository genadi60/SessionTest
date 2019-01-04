using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SessionTest.DataServices.Contracts;
using SessionTest.InputModels;
using SessionTest.Models;
using SessionTest.ViewModels;

namespace SessionTest.Controllers
{
    public class OrderController : BaseController
    {
        private readonly IOrdersService _ordersService;

        private readonly SignInManager<IdentityUser> _signInManager;

        private readonly UserManager<IdentityUser> _userManager;

        public OrderController(IOrdersService ordersService, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _ordersService = ordersService;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [Authorize]
        public IActionResult Create(string id)
        {
            

            var model = new 
            {
                CartId = id
            };

            return View("/views/shipping/index.cshtml", model);
        }

        [HttpPost]
        public IActionResult Create(ShippingDataInputModel model)
        {
            var userId = _userManager.GetUserId(User);
            

            //_ordersService.Create(HttpContext, model, userId);


            return RedirectToAction("Payment");
        }

        public IActionResult Payment()
        {
            return View();
        }

        public IActionResult Details(string id)
        {
            var order = _ordersService.GetById(id);
            var model = Mapper.Map<OrderViewModel>(order);

            return View(model);
        }
    }
}
