using Microsoft.AspNetCore.Http;
using SessionTest.Models;

namespace SessionTest.DataServices.Contracts
{
    public interface ICartsService
    {
        CartViewModel GetShoppingCart(HttpContext context);

        CartViewModel AddToShoppingCart(HttpContext context, string productId, int quantity);

        CartViewModel RemoveFromShoppingCart(HttpContext context, string productId, int quantity);
    }
}
