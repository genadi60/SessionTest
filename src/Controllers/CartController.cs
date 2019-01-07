using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SessionTest.DataServices.Contracts;
using SessionTest.ViewModels;
using Constants = SessionTest.Common.Constants;

namespace SessionTest.Controllers
{
    public class CartController : BaseController
    {
        private readonly ICartsService _cartsService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private CodeViewModel _code;

        public CartController(ICartsService cartsService, UserManager<IdentityUser> userManager, CodeViewModel code, SignInManager<IdentityUser> signInManager)
        {
            _cartsService = cartsService;
            _userManager = userManager;
            _code = code;
            _signInManager = signInManager;
        }

        public IActionResult Index(CodeViewModel model = null)
        {
            _code.Id = _userManager.GetUserId(User)?? model?.Id;

            if (_signInManager.IsSignedIn(User))
            {
                _code.IsAuthorized = true;
            }
            
            if (_code.Id == null)
            {
                _code.Id = Guid.NewGuid().ToString();

                return RedirectToAction("Code");
            }

            bool isValid = true;

            isValid = _cartsService.GetValidate(HttpContext, _code.Id);

            if (!isValid)
            {
                return RedirectToAction("Create", _code);
            }
            
            var cartModel = _cartsService.GetShoppingCart(HttpContext, _code.Id);

            if (TempData["Product"] != null)
            {
                var id = TempData["Product"].ToString();
                var quantity = int.Parse(TempData["Quantity"].ToString());

                cartModel = _cartsService.AddToShoppingCart(HttpContext, id, quantity, cartModel.Id).Result;
            }

            return View(cartModel);
        }
        
        [HttpPost]
        public IActionResult Add(string id, int quantity)
        {
            string cartid = _code.Id;
            
            if (cartid == null)
            {
                TempData["Product"] = id;
                TempData["Quantity"] = quantity;

                return RedirectToAction("Index");
            }

            var cartModel = _cartsService.AddToShoppingCart(HttpContext, id, quantity, cartid).Result;

            return View("Index", cartModel);
        }
        
        public IActionResult Remove(string orderId, int quantity)
        {
            string cartId = _code.Id;

            var cartModel = _cartsService.RemoveFromShoppingCart(HttpContext, orderId, quantity, cartId).Result;

            return View("Index", cartModel);
        }

        [HttpPost]
        public IActionResult Finish(string cartId)
        {
            return RedirectToAction("Index", "Shipping", new{id = cartId});
        }
        
        [HttpPost]
        public IActionResult ConfirmDelete(string cartId)
        {
            return RedirectToAction("Delete", new {cartId});
        }

        public IActionResult Delete(string cartId)
        {
            _cartsService.Delete(HttpContext, cartId);

            return View();
        }



        public IActionResult Create(CodeViewModel model)
        {
            _code = model;
            
            if (_code.Guest != null)
            {
                bool isValid = true;

                isValid = _cartsService.GetValidate(HttpContext, _code.Guest);

                if (isValid)
                {
                    _code.Id = _code.Guest;
                    _code.Message = null;
                }
                else
                {
                    _code.Message = Constants.INVALID_CODE_MESSAGE;
                    _code.Guest = null;
                    return RedirectToAction("Code", _code);
                }
            }
            else
            {
                bool isCreated = _cartsService.Create(HttpContext, _code).Result;

                if (!isCreated && TempData["Product"] == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                
            }
            
            return RedirectToAction("Index", _code);
        }

        public IActionResult Code()
        {
            return View(_code);
        }
    }
}
