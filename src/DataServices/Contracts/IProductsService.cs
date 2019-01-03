using System.Collections.Generic;
using System.Threading.Tasks;

namespace SessionTest.DataServices.Contracts
{
    public interface IProductsService
    {
        IEnumerable<TEntityViewModel> GetAll<TEntityViewModel>();

        Task<string> Create(int categoryId, string name, string description, decimal price);

        Task<string> Edit(string id, string categoryName, string name, string description, decimal price);

        Task Delete(string id);

        TEntityViewModel GetProductById<TEntityViewModel>(string id);

        IEnumerable<TEntityViewModel> GetAllByCategory<TEntityViewModel>(int categoryId);

        void InitialProducts();

        bool AddRatingToProduct(string productId, int rating);
    }
}
