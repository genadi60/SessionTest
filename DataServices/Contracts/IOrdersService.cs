using SessionTest.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SessionTest.DataServices.Contracts
{
    public interface IOrdersService
    {
        Task<string> Create(HttpContext context, ShipmentDataInputModel model, string userId = null);
    }
}
