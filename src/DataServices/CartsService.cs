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

        private readonly IRepository<CartProduct> _cartProductRepository;
        
        public CartsService(IRepository<Product> productRepository, IRepository<Cart> cartRepository, IRepository<CartProduct> cartProductRepository)
        {
            _productRepository = productRepository;
            _cartRepository = cartRepository;
            _cartProductRepository = cartProductRepository;
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

            var cartProduct = _cartProductRepository.All().FirstOrDefault(cp => cp.CartId.Equals(id) && cp.ProductId.Equals(productId));

            if (cartProduct == null)
            {
                var cpr = new CartProduct
                {
                    CartId = id,
                    ProductId = productId
                };

                await _cartProductRepository.AddAsync(cpr);
                await _cartProductRepository.SaveChangesAsync();
            }
            
            product.Unit -= quantity;
            product.TempUnit += quantity;
            
            _productRepository.Update(product);
            await _productRepository.SaveChangesAsync();

            var productInCart = cart.Products.FirstOrDefault(p => p.Id.Equals(productId));

            cart.Products.Remove(productInCart);

            if (productInCart == null)
            {
                product.Quantity = quantity;
                cart.Products.Add(product);
            }
            else
            {
                productInCart.Quantity += quantity;
                productInCart.Unit -= quantity;
                productInCart.TempUnit += quantity;

                cart.Products.Add(productInCart);
            }

            _cartRepository.Update(cart);
            await _cartRepository.SaveChangesAsync();

            var cartModel = Mapper.Map<CartViewModel>(cart);

            SessionExtensions.Set(context.Session, id, cartModel);

            return cartModel;
        }

        public CartViewModel RemoveFromShoppingCart(HttpContext context, string productId, int quantity, string id)
        {
            CartViewModel cartModel;
            var idProduct = productId;
            
            if (SessionExtensions.Get<CartViewModel>(context.Session, id) == null)
            {
                cartModel = new CartViewModel()
                {
                    Id = Guid.NewGuid().ToString()
                };

                SessionExtensions.Set(context.Session, id, cartModel);
            }

            cartModel = SessionExtensions.Get<CartViewModel>(context.Session, id);

            ICollection<ProductViewModel> products = cartModel.Products;

            var product = _productRepository.All()
                .FirstOrDefault(p => p.Id.Equals(idProduct));

            product.Unit += quantity;
            product.TempUnit -= quantity;

            _productRepository.Update(product);
            _productRepository.SaveChangesAsync();

            ProductViewModel productInCart;

            if (products.Count > 0)
            {
                productInCart = products.FirstOrDefault(p => p.Id.Equals(idProduct));

                productInCart.Quantity -= quantity;
                productInCart.Unit += quantity;


                if (productInCart.Quantity == 0)
                {
                    products.Remove(productInCart);
                }
            }

            cartModel = new CartViewModel()
            {
                Id = id,
                Products = products,
            };

            SessionExtensions.Set(context.Session, id, cartModel);

            var cart = _cartRepository.All().FirstOrDefault(c => c.Id.Equals(id));
            //cart.Products = cartModel.Products.Select(p => Mapper.Map<Product>(p)).ToList();

            _cartRepository.Update(cart);
            _cartRepository.SaveChangesAsync();

            return cartModel;
        }

        [HttpPost]
        public async Task<string> Finish(HttpContext context, string id)
        {
            CartViewModel model;

            if (SessionExtensions.Get<CartViewModel>(context.Session, id) == null)
            {
                model = new CartViewModel()
                {
                    Id = Guid.NewGuid().ToString()
                };

                SessionExtensions.Set(context.Session, id, model);
            }

            model = SessionExtensions.Get<CartViewModel>(context.Session, id);

            ICollection<ProductViewModel> products = model.Products;

            var cart = new Cart
            {
                Id = id,
                Products = products.Select(p => Mapper.Map<Product>(p)).ToList(),
            };

            await _cartRepository.AddAsync(cart);
            await _cartRepository.SaveChangesAsync();

            return cart.Id;
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
