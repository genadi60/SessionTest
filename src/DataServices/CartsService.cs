using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SessionTest.Common;
using SessionTest.DataServices.Contracts;
using SessionTest.Models;
using SessionTest.ViewModels;
using SessionExtensions = SessionTest.Common.SessionExtensions;

namespace SessionTest.DataServices
{
    public class CartsService : ICartsService
    {
        private readonly IRepository<Product> _productRepository;

        private readonly IRepository<Cart> _cartRepository;

        private readonly IRepository<Order> _orderRepository;

        private readonly IRepository<CartOrder> _cartOrderRepository;
        
        public CartsService(IRepository<Product> productRepository, 
            IRepository<Cart> cartRepository, 
            IRepository<CartOrder> cartOrderRepository, 
            IRepository<Order> orderRepository)
        {
            _productRepository = productRepository;
            _cartRepository = cartRepository;
            _cartOrderRepository = cartOrderRepository;
            _orderRepository = orderRepository;
        }

        [Authorize]
        public CartViewModel GetShoppingCart(HttpContext context, string id)
        {
            CartViewModel cartModel;

            var cart = _cartRepository.All()
                .FirstOrDefault(c => c.Id.Equals(id));

            if (cart == null)
            {
                if (SessionExtensions.Get<CartViewModel>(context.Session, id) == null)
                {
                    cartModel = new CartViewModel()
                    {
                        Id = id
                    };

                    SessionExtensions.Set(context.Session, id, cartModel);
                }
            }
            else if(SessionExtensions.Get<CartViewModel>(context.Session, id) == null)
            {
                return null;
            }

            if (!_cartRepository.All().Any(c => c.Id.Equals(id)))
            {
                cart = new Cart
                {
                    Id = id
                };

                _cartRepository.AddAsync(cart);
                _cartRepository.SaveChangesAsync();
            }

            cartModel = SessionExtensions.Get<CartViewModel>(context.Session, id);
            
            return cartModel;
        }

        public async Task<CartViewModel> AddToShoppingCart(HttpContext context, string productId, int quantity, string id)
        {
            var cart = _cartRepository.All().FirstOrDefault(c => c.Id.Equals(id));

            var product = _productRepository.All()
                .FirstOrDefault(p => p.Id.Equals(productId));

            var order = new Order
            {
                Id = Guid.NewGuid().ToString(),
                ProductId = product.Id,
                Product = product,
                Quantity = quantity,
                Price = product.Price
            };

            product.Unit -= quantity;

            
            _productRepository.Update(product);
            await _productRepository.SaveChangesAsync();

            await _orderRepository.AddAsync(order);
            await _orderRepository.SaveChangesAsync();

            var ordersId = _cartOrderRepository.All().Where(co => co.CartId.Equals(id)).Select(co => co.OrderId).ToList();

            var orders = _orderRepository.All().Where(o => ordersId.Contains(o.Id)).ToList();

            cart.Orders = orders;
            
            _cartRepository.Update(cart);
            await _cartRepository.SaveChangesAsync();

            var cartOrder = new CartOrder
            {
                Id = Guid.NewGuid().ToString(),
                CartId = id,
                OrderId = order.Id
            };

            await _cartOrderRepository.AddAsync(cartOrder);
            await _cartOrderRepository.SaveChangesAsync();

            var cartModel = Mapper.Map<CartViewModel>(cart);

            SessionExtensions.Set(context.Session, id, cartModel);

            return cartModel;
        }

        public async Task<CartViewModel> RemoveFromShoppingCart(HttpContext context, string productId, int quantity, string id)
        {
            var cart = _cartRepository.All().FirstOrDefault(c => c.Id.Equals(id));

            var product = _productRepository.All()
                .FirstOrDefault(p => p.Id.Equals(productId));

            var order = cart.Orders.FirstOrDefault(o => o.ProductId.Equals(productId));

            cart.Orders.Remove(order);

            product.Unit += quantity;

            var cartOrder = _cartOrderRepository.All()
                .FirstOrDefault(co => co.CartId.Equals(cart.Id) && co.OrderId.Equals(order.Id));

            _cartOrderRepository.Delete(cartOrder);
            await _cartOrderRepository.SaveChangesAsync();

            _productRepository.Update(product);
            await _productRepository.SaveChangesAsync();

            await _orderRepository.AddAsync(order);
            await _orderRepository.SaveChangesAsync();

            cart.Orders.Add(order);
            _cartRepository.Update(cart);
            await _cartRepository.SaveChangesAsync();

            var cartModel = Mapper.Map<CartViewModel>(cart);

            SessionExtensions.Set(context.Session, id, cartModel);

            return cartModel;
        }

        //[HttpPost]
        public async Task<string> Finish(HttpContext context, string id)
        {
        //    CartViewModel model;

        //    if (SessionExtensions.Get<CartViewModel>(context.Session, id) == null)
        //    {
        //        model = new CartViewModel()
        //        {
        //            Id = Guid.NewGuid().ToString()
        //        };

        //        SessionExtensions.Set(context.Session, id, model);
        //    }

        //    model = SessionExtensions.Get<CartViewModel>(context.Session, id);

        //    ICollection<ProductViewModel> products = model.Products;

        //    var cart = new Cart
        //    {
        //        Id = id,
        //        Products = products.Select(p => Mapper.Map<Product>(p)).ToList(),
        //    };

        //    await _cartRepository.AddAsync(cart);
        //    await _cartRepository.SaveChangesAsync();

        //    return cart.Id;
        return "";
        }

        public async Task Delete(HttpContext context, string id)
        {

            //var cart = _cartRepository.All().FirstOrDefault(c => c.Id.Equals(id));
            //var productsInCart = cart.Products;
            //var productsInCartId = productsInCart.Select(p => p.Id).ToList();

            //var products = _productRepository.All().Where(p => productsInCartId.Contains(p.Id)).ToList();

            //if (products.Count > 0)
            //{
            //    foreach (var product in products)
            //    {
            //        var productModel = productsInCart.Single(p => p.Id.Equals(product.Id));
            //        product.Unit += productModel.Quantity;
            //        product.TempUnit -= productModel.Quantity;
            //    }

            //    _productRepository.UpdateRange(products);
            //    await _productRepository.SaveChangesAsync();
            //}

            //context.Session.Remove(id);

            //_cartRepository.Delete(cart);
            //await _cartRepository.SaveChangesAsync();
        }

        public bool GetValidate(HttpContext context, string id)
        {
            return SessionExtensions.Get<CartViewModel>(context.Session, id) != null;
        }

        public async Task<bool> Create(HttpContext context, string id)
        {
            var cart = new Cart
            {
                Id = id
            };

            await _cartRepository.AddAsync(cart);
            await _cartRepository.SaveChangesAsync();

            var model = new CartViewModel()
            {
                Id = id
            };

            SessionExtensions.Set(context.Session, id, model);

            return SessionExtensions.Get<CartViewModel>(context.Session, id) != null;
        }
    }
}
