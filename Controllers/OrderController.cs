using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SessionTest.DataServices.Contracts;
using SessionTest.Models;

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
            bool isAuthenticated = _signInManager.IsSignedIn(User);

            var model = new ShipmentDataInputModel
            {
                CartId = id
            };

            return View("/views/shipment/index.cshtml", model);
        }

        [HttpPost]
        public IActionResult Create(ShipmentDataInputModel model)
        {
            var userId = _userManager.GetUserId(User);


            _ordersService.Create(HttpContext, model, userId);


            return View("/views/shipment/index.cshtml");
        }
    }
}
