using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SessionTest.Common;
using SessionTest.DataServices.Contracts;
using SessionTest.Models;
using SessionExtensions = SessionTest.Common.SessionExtensions;

namespace SessionTest.DataServices
{
    public class CartsService : ICartsService
    {
        private readonly IRepository<Product> _productRepository;

        public CartsService(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public CartViewModel GetShoppingCart(HttpContext context)
        {

            if (SessionExtensions.Get<Cart>(context.Session, "Cart") == null)
            {
                var cart = new Cart
                {
                    Id = Guid.NewGuid().ToString()
                };

                SessionExtensions.Set(context.Session, "Cart", cart);
            }

            dynamic cartObject = SessionExtensions.Get<Cart>(context.Session, "Cart") == null
                ? null
                : JsonConvert.DeserializeObject<Cart>(context.Session.GetString("Cart"));

            string id = cartObject.Id;

            ICollection<ProductViewModel> products = cartObject.Products;

            var model = new CartViewModel
            {
                Id = id,
                Products = products,
                Total = products.Sum(p => p.Price * p.Quantity)
            };

            return model;
        }

        public CartViewModel AddToShoppingCart(HttpContext context, string productId, int quantity)
        {
            Cart cart;
            var id = productId;
            if (quantity == 0)
            {
                quantity = 1;
            }
            if (SessionExtensions.Get<Cart>(context.Session, "Cart") == null)
            {
                cart = new Cart
                {
                    Id = Guid.NewGuid().ToString()
                };

                SessionExtensions.Set(context.Session, "Cart", cart);
            }

            dynamic cartObject = context.Session.GetString("Cart") == null
                ? null
                : JsonConvert.DeserializeObject<Cart>(context.Session.GetString("Cart"));

            string cartId = cartObject.Id;

            ICollection<ProductViewModel> products = cartObject.Products;

            var product = _productRepository.All()
                .FirstOrDefault(p => p.Id.Equals(id));

            product.Unit -= quantity;
            product.Quantity = quantity;

            _productRepository.Update(product);
            _productRepository.SaveChangesAsync();

            var productInCart = products.FirstOrDefault(p => p.Id.Equals(id));

            if (productInCart == null)
            {
                var productViewModel = Mapper.Map<ProductViewModel>(product);
                products.Add(productViewModel);
            }
            else
            {
                productInCart.Quantity += quantity;
                productInCart.Unit -= quantity;
            }
            
            cart = new Cart
            {
                Id = cartId,
                Products = products
            };

            SessionExtensions.Set(context.Session, "Cart", cart);

            var model = new CartViewModel
            {
                Id = cartId,
                Products = products,
                Total = products.Sum(p => p.Price * p.Quantity)
            };

            return model;
        }

        public CartViewModel RemoveFromShoppingCart(HttpContext context, string productId, int quantity)
        {
            Cart cart;
            var id = productId;
            if (quantity == 0)
            {
                quantity = 1;
            }

            if (SessionExtensions.Get<Cart>(context.Session, "Cart") == null)
            {
                cart = new Cart
                {
                    Id = Guid.NewGuid().ToString()
                };

                SessionExtensions.Set(context.Session, "Cart", cart);
            }

            dynamic cartObject = context.Session.GetString("Cart") == null
                ? null
                : JsonConvert.DeserializeObject<Cart>(context.Session.GetString("Cart"));

            string cartId = cartObject.Id;

            ICollection<ProductViewModel> products = cartObject.Products;

            var product = _productRepository.All()
                .FirstOrDefault(p => p.Id.Equals(id));

            product.Unit += quantity;

            _productRepository.Update(product);
            _productRepository.SaveChangesAsync();

            var productInCart = products.FirstOrDefault(p => p.Id.Equals(id));

            productInCart.Quantity -= quantity;
            productInCart.Unit += quantity;

            if (productInCart.Quantity == 0)
            {
                products.Remove(productInCart);
            }

            cart = new Cart
            {
                Id = cartId,
                Products = products
            };

            SessionExtensions.Set(context.Session, "Cart", cart);

            var model = new CartViewModel
            {
                Id = cartId,
                Products = products,
                Total = products.Sum(p => p.Price * p.Quantity)
            };

            return model;
        }
    }
}
