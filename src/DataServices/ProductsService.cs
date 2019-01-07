using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SessionTest.Common;
using SessionTest.DataServices.Contracts;
using SessionTest.MappingServices;
using SessionTest.Models;

namespace SessionTest.DataServices
{
    public class ProductsService : IProductsService
    {
        private readonly IRepository<Product> _productsRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Cart> _cartRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<CartOrder> _cartOrderRepository;
        private readonly IImagesService _imagesService;
        

        public ProductsService(IRepository<Product> productsRepository, 
            IImagesService imagesService, 
            IRepository<Category> categoryRepository, 
            IRepository<Cart> cartRepository, 
            IRepository<Order> orderRepository, IRepository<CartOrder> cartOrderRepository)
        {
            _productsRepository = productsRepository;
            _imagesService = imagesService;
            _categoryRepository = categoryRepository;
            _cartRepository = cartRepository;
            _orderRepository = orderRepository;
            _cartOrderRepository = cartOrderRepository;
        }

        public IEnumerable<ProductViewModel> GetAll<ProductViewModel>() => _productsRepository.All().To<ProductViewModel>();
        
        public async Task<string> Create(int categoryId, string name, string description, decimal price)
        {
            var product = new Product()
            {
                Id = Guid.NewGuid().ToString(),
                CategoryId = categoryId,
                Name = name,
                Description = description,
                Price = price
            };

            await _productsRepository.AddAsync(product);
            await _productsRepository.SaveChangesAsync();

            return product.Id;
        }

        public async Task<string> Edit(string id, string categoryName, string name, string description, decimal price)
        {
            var product = _productsRepository.All()
                .FirstOrDefault(p => p.Id == id);

            var category = _categoryRepository.All()
                .FirstOrDefault(c => c.Name.Equals(categoryName));

            if (product == null)
            {
                throw new KeyNotFoundException();
            }

            product.Category = category;
            product.Name = name;
            product.Description = description;
            product.Price = price;

            _productsRepository.Update(product);

            await _productsRepository.SaveChangesAsync();

            return product.Id;
        }

        public async Task Delete(string id)
        {
            var product = _productsRepository.All().FirstOrDefault(x => x.Id == id);

            _productsRepository.Delete(product);
            await _productsRepository.SaveChangesAsync();
        }

        public TEntityViewModel GetProductById<TEntityViewModel>(string id)
        {
            var productViewModel = _productsRepository.All()
                .Where(x => x.Id == id)
                .To<TEntityViewModel>()
                .FirstOrDefault();

            return productViewModel;
        }

        public IEnumerable<TEntityViewModel> GetAllByCategory<TEntityViewModel>(int categoryId) 
            => _productsRepository.All()
                .Where(p => p.CategoryId == categoryId)
                .To<TEntityViewModel>();

        

        public bool AddRatingToProduct(string productId, int rating)
        {
            return true;
        }
    }
}
