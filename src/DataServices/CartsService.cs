using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SessionTest.Common;
using SessionTest.DataServices.Contracts;
using SessionTest.MappingServices;
using SessionTest.Models;
using SessionTest.ViewModels;

namespace SessionTest.DataServices
{
    public class CartsService : ICartsService
    {
        private readonly IRepository<Product> _productRepository;

        private readonly IRepository<Cart> _cartRepository;

        private readonly IRepository<Order> _orderRepository;

        private readonly IRepository<CartOrder> _cartOrderRepository;

        private readonly IOrdersService _ordersService;

        private static readonly object Locker = new object();

        public CartsService(IRepository<Product> productRepository,
            IRepository<Cart> cartRepository,
            IRepository<CartOrder> cartOrderRepository,
            IRepository<Order> orderRepository, 
            IOrdersService ordersService)
        {
            _productRepository = productRepository;
            _cartRepository = cartRepository;
            _cartOrderRepository = cartOrderRepository;
            _orderRepository = orderRepository;
            _ordersService = ordersService;
        }


        public CartViewModel GetShoppingCart(HttpContext context, string id)
        {

            var cartModel = _cartRepository.All()
                .Include(c => c.Orders)
                .ThenInclude(o => o.Product)
                .To<CartViewModel>()
                .FirstOrDefault(c => c.Id.Equals(id));

            return cartModel;
        }

        public async Task<CartViewModel> AddToShoppingCart(HttpContext context, string productId, int quantity, string id)
        {
            var cart = _cartRepository.All().FirstOrDefault(c => c.Id.Equals(id));

            var orders = _orderRepository.All().Where(o => o.CartId.Equals(id)).Include(o => o.Product).ToList();

            Order order;
            Product product;

            if (orders.Count > 0 && orders.Any(o => o.ProductId.Equals(productId)))
            {

                order = orders.FirstOrDefault(o => o.ProductId.Equals(productId));
                orders.Remove(order);

                product = _productRepository.All().FirstOrDefault(p => p.Id.Equals(order.ProductId));
                product.Unit -= quantity;

                _productRepository.Update(product);
                await _productRepository.SaveChangesAsync();

                order.Product = product;
                order.Quantity += quantity;

                _orderRepository.Update(order);
                await _orderRepository.SaveChangesAsync();

            }
            else
            {
                product = _productRepository.All()
                    .FirstOrDefault(p => p.Id.Equals(productId));

                product.Unit -= quantity;

                order = _ordersService.CreateOrder(cart, product, quantity).Result;

                _productRepository.Update(product);
                await _productRepository.SaveChangesAsync();

                var cartOrder = new CartOrder
                {
                    Id = Guid.NewGuid().ToString(),
                    CartId = id,
                    OrderId = order.Id
                };

                await _cartOrderRepository.AddAsync(cartOrder);
                await _cartOrderRepository.SaveChangesAsync();
            }

            orders.Add(order);

            cart.Orders = orders;

            _cartRepository.Update(cart);
            await _cartRepository.SaveChangesAsync();

            var cartModel = Mapper.Map<CartViewModel>(cart);

            return cartModel;
        }

        public async Task<CartViewModel> RemoveFromShoppingCart(HttpContext context, string orderId, int quantity, string id)
        {
            var cart = _cartRepository.All().FirstOrDefault(c => c.Id.Equals(id));

            var orders = _orderRepository.All().Include(o => o.Product).Where(o => o.CartId.Equals(id)).ToList();

            var order = orders.FirstOrDefault(o => o.Id.Equals(orderId));

            orders.Remove(order);

            var product = order.Product;

            var cartOrder = _cartOrderRepository.All().FirstOrDefault(co => co.CartId.Equals(id) && co.OrderId.Equals(order.Id));

            product.Unit += quantity;

            order.Quantity -= quantity;


            _productRepository.Update(product);

            await _productRepository.SaveChangesAsync();


            if (order.Quantity <= 0)
            {
                _orderRepository.Delete(order);
                cart.Orders.Remove(order);

                _cartOrderRepository.Delete(cartOrder);
                await _cartOrderRepository.SaveChangesAsync();
            }
            else
            {
                orders.Add(order);

                _orderRepository.Update(order);
                await _orderRepository.SaveChangesAsync();
            }

            cart.Orders = orders;

            _cartRepository.Update(cart);
            await _cartRepository.SaveChangesAsync();

            var cartModel = Mapper.Map<CartViewModel>(cart);

            return cartModel;
        }

        public async Task Delete(HttpContext context, string id)
        {

            var cart = _cartRepository.All().FirstOrDefault(c => c.Id.Equals(id));

            var orders = _orderRepository.All().Include(o => o.Product).Where(o => o.CartId.Equals(id)).ToList();

            IList<CartOrder> cartOrders = new List<CartOrder>();

            if (_cartOrderRepository.All().Any(co => co.CartId.Equals(id)))
            {
                cartOrders = _cartOrderRepository.All().Where(co => co.CartId.Equals(id)).ToList();
            }

            var products = new List<Product>();

            foreach (var order in orders)
            {
                var product = order.Product;
                product.Unit += order.Quantity;
                products.Add(product);
            }

            _productRepository.UpdateRange(products);
            await _productRepository.SaveChangesAsync();

            if (cartOrders.Count > 0)
            {
                _cartOrderRepository.DeleteRange(cartOrders);
                await _cartOrderRepository.SaveChangesAsync();
            }

            _orderRepository.DeleteRange(orders);
            await _orderRepository.SaveChangesAsync();

            _cartRepository.Delete(cart);
            await _cartRepository.SaveChangesAsync();

            context.Session.Clear();
        }

        public bool GetValidate(HttpContext context, string id)
        {
            return _cartRepository.All().Any(c => c.Id.Equals(id));
        }

        public async Task<bool> Create(HttpContext context, CodeViewModel codeModel)
        {
            if (_cartRepository.All().Any(c => c.Id.Equals(codeModel.Id)))
            {
                return true;
            }

            var cart = new Cart
            {
                Id = codeModel.Id,
                IsAuthorized = codeModel.IsAuthorized
            };

            await _cartRepository.AddAsync(cart);
            await _cartRepository.SaveChangesAsync();

            var model = new CartViewModel()
            {
                Id = codeModel.Id
            };
            if (context.Session.Get<CartViewModel>(codeModel.Id) == null)
            {
                context.Session.Set<CartViewModel>(codeModel.Id, model);
            }
            

            var sessionModel = context.Session.Get<CartViewModel>(codeModel.Id);

            return sessionModel != null;
        }
    }
}
