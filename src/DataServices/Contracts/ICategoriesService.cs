using SessionTest.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using SessionTest.ViewModels;

namespace SessionTest.DataServices.Contracts
{
    public interface ICategoriesService
    {
        IEnumerable<CategoryViewModel> GetAll();

        Task<int> Create(string name);

        bool IsCategoryIdValid(int categoryId);

        int GetCategoryId(string name);

        IEnumerable<ProductViewModel> GetAllProductsFromCategory(int categoryId);
    }
}
