using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SessionTest.DataServices.Contracts;

namespace SessionTest.Controllers
{
    public class CartController : BaseController
    {
        private readonly ICartsService _cartsService;

        public CartController(ICartsService cartsService)
        {
            _cartsService = cartsService;
        }


        public IActionResult Index()
        {
            var cartModel = _cartsService.GetShoppingCart(HttpContext);

            return View(cartModel);
        }

        [HttpPost]
        public IActionResult AddToCart(string productId, int quantity)
        {

            var cartModel = _cartsService.AddToShoppingCart(HttpContext, productId, quantity);

            return View("Index", cartModel);
        }

        public IActionResult Remove(string productId, int quantity)
        {
            var cartModel = _cartsService.RemoveFromShoppingCart(HttpContext, productId, quantity);

            return View("Index", cartModel);
        }

    }
}
