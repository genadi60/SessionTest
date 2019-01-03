using SessionTest.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SessionTest.InputModels;

namespace SessionTest.DataServices.Contracts
{
    public interface IOrdersService
    {
        Task<string> Create(HttpContext context, ShippingDataInputModel model, string userId = null);
    }
}
