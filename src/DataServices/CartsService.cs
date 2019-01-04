﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

            var cart = _cartRepository.All().FirstOrDefault(c => c.Id.Equals(id));

            var orders = _orderRepository.All().Where(o => o.CartId.Equals(id)).Include(o => o.Product).ToList();

            cart.Orders = orders;

            var cartModel = Mapper.Map<CartViewModel>(cart);

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

                order = _ordersService.CreateOrder(cart, product, quantity).Result;

                product.Unit -= quantity;

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

            //SessionExtensions.Set(context.Session, id, cartModel);

            return cartModel;
        }

        
        public async Task<string> FinishCart(HttpContext context, string id)
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
            return _cartRepository.All().Any(c => c.Id.Equals(id));
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
