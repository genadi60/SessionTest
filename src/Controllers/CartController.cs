using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
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

        public IActionResult Index(CodeViewModel model)
        {
            _code.Id = _userManager.GetUserId(User)??model.Id;

            if (_signInManager.IsSignedIn(User))
            {
                _code.IsAuthorized = true;
            }
            
            if (_code.Id == null)
            {
                _code.Id = Guid.NewGuid().ToString();

                return RedirectToAction("Code", _code);
            }

            var cartModel = _cartsService.GetShoppingCart(HttpContext, _code.Id);

            if (TempData["Product"] != null)
            {
                return RedirectToAction("Add", model);
            }

            return View(cartModel);
        }
        
        public IActionResult Add(CodeViewModel model, string productId, int quantity)
        {
            string id = _code.Id;

            if (id == null)
            {
                TempData["Product"] = productId;
                TempData["Quantity"] = quantity;

                return RedirectToAction("Index");
            }

            if (TempData["Product"] != null)
            {
                productId = TempData["Product"].ToString();
                quantity = int.Parse(TempData["Quantity"].ToString());
            }


            var cartModel = _cartsService.AddToShoppingCart(HttpContext, productId, quantity, id).Result;

            return View("Index", cartModel);
        }

        
        public IActionResult Remove(CodeViewModel model, string orderId, int quantity)
        {
            string id = _code.Id;

            var cartModel = _cartsService.RemoveFromShoppingCart(HttpContext, orderId, quantity, id).Result;

            return View("Index", cartModel);
        }

        [HttpPost]
        public IActionResult Finish(CodeViewModel model, string cartId)
        {
            return RedirectToAction("Index", "Shipping", new{id = cartId});
        }

        
        public IActionResult Delete(CodeViewModel model)
        {
            var id = _code.Id;

            _cartsService.Delete(HttpContext, id);

            return View("/views/cart/delete.cshtml");
        }

        
        public IActionResult Create(CodeViewModel model)
        {
            _code = model;
            bool isValid = true;

            if (_code.Guest != null)
            {
                isValid = _cartsService.GetValidate(HttpContext, _code.Guest);

                if (isValid)
                {
                    _code.Id = _code.Guest;
                    model.Id = _code.Id;
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
            
            return RedirectToAction("Index", model);
        }

        public IActionResult Code(CodeViewModel model)
        {
            return View(model);
        }
    }
}
