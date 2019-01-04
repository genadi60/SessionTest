using System.Collections.Generic;
using SessionTest.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SessionTest.InputModels;

namespace SessionTest.DataServices.Contracts
{
    public interface IOrdersService
    {
        Task<Order> CreateOrder(Cart cart, Product product, int quantity);

        Task InitialDatabase(ICollection<string> cartsId);

        Order GetById(string id);
    }
}
