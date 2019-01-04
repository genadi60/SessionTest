using Microsoft.AspNetCore.Http;
using SessionTest.Models;
using SessionTest.ViewModels;
using System.Threading.Tasks;

namespace SessionTest.DataServices.Contracts
{
    public interface ICartsService
    {
        CartViewModel GetShoppingCart(HttpContext context, string id);

        Task<CartViewModel> AddToShoppingCart(HttpContext context, string productId, int quantity, string id);

        Task<CartViewModel> RemoveFromShoppingCart(HttpContext context, string productId, int quantity, string id);

        Task<string> FinishCart(HttpContext context, string id);

        Task Delete(HttpContext context, string id);

        bool GetValidate(HttpContext context, string id);

        Task<bool> Create(HttpContext context, string id);
    }
}
