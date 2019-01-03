using SessionTest.Common;
using SessionTest.DataServices.Contracts;
using SessionTest.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SessionTest.MappingServices;
using SessionTest.ViewModels;

namespace SessionTest.DataServices
{
    public class CategoriesService : ICategoriesService
    {
        private readonly IRepository<Category> _categoriesRepository;
        private readonly IProductsService _productsService;

        public CategoriesService(IRepository<Category> categoriesRepository, IProductsService productsService)
        {
            _categoriesRepository = categoriesRepository;
            _productsService = productsService;
        }

        public IEnumerable<CategoryViewModel> GetAll()
        {
            var categories = _categoriesRepository.All().OrderBy(x => x.Name)
                .To<CategoryViewModel>().ToList();

            return categories;
        }
        
        public async Task<int> Create(string name)
        {
            var category = new Category
            {
                Name = name
            };

            await _categoriesRepository.AddAsync(category);
            await _categoriesRepository.SaveChangesAsync();

            return category.Id;
        }

        public bool IsCategoryIdValid(int categoryId) => _categoriesRepository.All().Any(x => x.Id == categoryId);

        public int GetCategoryId(string name)
        {
            var category = _categoriesRepository.All().FirstOrDefault(x => x.Name == name);
            if (category == null)
            {
                throw new KeyNotFoundException();
            }
            return category.Id;
        }

        public IEnumerable<ProductViewModel> GetAllProductsFromCategory(int categoryId)
        {
            if (!IsCategoryIdValid(categoryId))
            {
                throw new KeyNotFoundException();
            }

            return _productsService.GetAllByCategory<ProductViewModel>(categoryId);
        }
    }
}
