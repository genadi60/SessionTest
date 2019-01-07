using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SessionTest.DataServices.Contracts;
using SessionTest.InputModels;

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

        public IActionResult Create(string id)
        {
            var model = new ShippingDataInputModel
            {
                CartId = id
            };

            return View("/views/shipping/index.cshtml", model);
        }

        [HttpPost]
        public IActionResult Details(string id)
        {
            return RedirectToAction("ConfirmDetails", new { id = id });
        }

        public IActionResult ConfirmDetails(string id)
        {
            var order = _ordersService.GetOrderViewModel(id);

            return View("Details", order);
        }
    }
}
