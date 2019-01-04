using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SessionTest.Common;
using SessionTest.DataServices.Contracts;
using SessionTest.InputModels;
using SessionTest.Models;
using SessionTest.ViewModels;

namespace SessionTest.Controllers
{
    public class PaymentController : BaseController
    {
        private readonly IPaymentsService _paymentsService;
        private readonly IRepository<Cart> _cartRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public PaymentController(IPaymentsService paymentsService, UserManager<IdentityUser> userManager, IRepository<Cart> cartRepository)
        {
            _paymentsService = paymentsService;
            _userManager = userManager;
            _cartRepository = cartRepository;
        }

        
        public IActionResult Index(ShippingDataInputModel shipping)
        {
            var model = _paymentsService.GetPayment(shipping);

            
           
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Confirm(PaymentMethodViewModel model)
        {
            var id = _userManager.GetUserId(User);
            var cart = _cartRepository.All().FirstOrDefault(c => c.Id.Equals(id));
            model.Total = cart.Total;

            return View($"/views/payment/{model.Name}.cshtml", model);
        }

        public IActionResult Finish()
        {
            return View();
        }
    }
}
