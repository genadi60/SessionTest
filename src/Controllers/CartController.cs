using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SessionTest.DataServices.Contracts;
using SessionTest.ViewModels;

namespace SessionTest.Controllers
{
    public class CartController : BaseController
    {
        private readonly ICartsService _cartsService;
        private readonly UserManager<IdentityUser> _userManager;
        private CodeViewModel _code;

        public CartController(ICartsService cartsService, UserManager<IdentityUser> userManager, CodeViewModel code)
        {
            _cartsService = cartsService;
            _userManager = userManager;
            _code = code;
        }

        public IActionResult Index(CodeViewModel model)
        {
            _code.Id = _userManager.GetUserId(User)??model.Id;

            if (_code.Id == null)
            {
                _code.Id = Guid.NewGuid().ToString();

                return RedirectToAction("Code", _code);
            }

            var cartModel = _cartsService.GetShoppingCart(HttpContext, _code.Id);

            return View(cartModel);
        }


        [HttpPost]
        public IActionResult AddToCart(string productId, int quantity)
        {
            string id = _code.Id;

            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var cartModel = _cartsService.AddToShoppingCart(HttpContext, productId, quantity, id).Result;

            return View("Index", cartModel);
        }

        [Authorize]
        public IActionResult Remove(string productId, int quantity)
        {
            string id = _userManager.GetUserId(User);

            var cartModel = _cartsService.RemoveFromShoppingCart(HttpContext, productId, quantity, id);

            return View("Index", cartModel);
        }

        [HttpPost]
        public IActionResult Finish()
        {
            var id = _userManager.GetUserId(User);

            _cartsService.Finish(HttpContext, id);

            return RedirectToAction("Index", "Payment");
        }

        [HttpPost]
        public IActionResult Delete()
        {
            var id = _userManager.GetUserId(User);

            _cartsService.Delete(HttpContext, id);

            return View("/views/cart/delete.cshtml");
        }

        [HttpPost]
        public IActionResult Create(CodeViewModel model)
        {
            _code = model;
            bool isValid = true;

            if (_code.Guest != null && _code.Message == null)
            {
                isValid = _cartsService.GetValidate(HttpContext, _code.Guest);

                if (isValid)
                {
                    _code.Id = _code.Guest;
                }
                else
                {
                    _code.Message = "Invalid code or session has expired. If you want a new cart, press continue";
                    return RedirectToAction("Code", _code);
                }
            }
            else
            {
                bool isCreated = _cartsService.Create(HttpContext, _code.Id).Result;

                if (!isCreated)
                {
                    return RedirectToAction("Index", "Home");
                }
                
            }
            
            return RedirectToAction("Index", model);
        }

        public IActionResult Code(CodeViewModel model)
        {
            return View(model);
        }
    }
}
