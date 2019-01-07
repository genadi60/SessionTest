using SessionTest.DataServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using SessionTest.MappingServices;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SessionTest.Common;
using SessionTest.Models;
using SessionTest.ViewModels;

namespace SessionTest.DataServices
{
    public class OrdersService : IOrdersService
    {
        private readonly IRepository<Order> _ordersRepository;
        private readonly IRepository<Cart> _cartsRepository;
        private readonly IRepository<CartOrder> _cartOrdersRepository;
        private readonly IRepository<Product> _productsRepository;

        public OrdersService(IRepository<Order> ordersRepository, IRepository<Product> productsRepository, IRepository<Cart> cartsRepository, IRepository<CartOrder> cartOrdersRepository)
        {
            _ordersRepository = ordersRepository;
            _productsRepository = productsRepository;
            _cartsRepository = cartsRepository;
            _cartOrdersRepository = cartOrdersRepository;
        }

        public async Task<Order> CreateOrder(Cart cart, Product product, int quantity)
        {
            var order = new Order
            {
                Id = Guid.NewGuid().ToString(),
                ProductId = product.Id,
                Product = product,
                CartId = cart.Id,
                Cart = cart,
                Quantity = quantity,
                Price = product.Price
            };

            await _ordersRepository.AddAsync(order);
            await _ordersRepository.SaveChangesAsync();

            return order;
        }

        public async Task InitialDatabase(ICollection<string> cartsId)
        {
            var cartsToDelete = _cartsRepository.All().Where(c => cartsId.Contains(c.Id) && c.IsAuthorized == false).ToList();

            var ordersToDelete = _ordersRepository.All().Where(o => cartsId.Contains(o.CartId)).Include(o => o.Product).ToList();

            var productsToUpdate = new List<Product>();

            foreach (var order in ordersToDelete)
            {
                var product = order.Product;
                product.Unit += order.Quantity;

                productsToUpdate.Add(product);
            }

            _productsRepository.UpdateRange(productsToUpdate);
            await _productsRepository.SaveChangesAsync();

            var cartOrdersToDelete = _cartOrdersRepository.All().Where(co => cartsId.Contains(co.CartId)).ToList();

            _cartOrdersRepository.DeleteRange(cartOrdersToDelete);
            await _cartOrdersRepository.SaveChangesAsync();

            _ordersRepository.DeleteRange(ordersToDelete);
            await _ordersRepository.SaveChangesAsync();

            _cartsRepository.DeleteRange(cartsToDelete);
            await _cartsRepository.SaveChangesAsync();
        }

        public Order GetById(string id)
        {
            return _ordersRepository.All().Include(o => o.Product).FirstOrDefault(o => o.Id.Equals(id));
        }

        public OrderViewModel GetOrderViewModel(string id)
        {
            var model = _ordersRepository.All().To<OrderViewModel>().FirstOrDefault(p => p.Id.Equals(id));

            return model;
        }
    }
}
