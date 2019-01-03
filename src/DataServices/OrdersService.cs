using SessionTest.DataServices.Contracts;
using System;
using System.Linq;
using SessionTest.MappingServices;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using SessionTest.Common;
using SessionTest.InputModels;
using SessionTest.Models;
using SessionTest.ViewModels;
using SessionExtensions = SessionTest.Common.SessionExtensions;

namespace SessionTest.DataServices
{
    public class OrdersService : IOrdersService
    {
        private readonly IRepository<Order> _orderRepository;

        private readonly IRepository<Product> _productRepository;

        public OrdersService(IRepository<Order> orderRepository, IRepository<Product> productRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        public async Task<string> Create(HttpContext context, ShippingDataInputModel model, string userId = null)
        {
            var id = model.CartId;
            var cart = SessionExtensions.Get<CartViewModel>(context.Session, userId);

            if (cart == null || !cart.Id.Equals(id))
            {
                return null;
            }

            
            var products = cart.Products.AsQueryable()
                .To<Product>()
                .ToList();

            var shippingData = Mapper.Map<ShippingData>(model);

            var order = new Order
            {
                Id = Guid.NewGuid().ToString(),
                Products = products,
                ClientId = userId,
                Total = cart.Total,
                ShippingData = shippingData
            };

            await _orderRepository.AddAsync(order);
            await _orderRepository.SaveChangesAsync();

            //foreach (var product in model.Products)
            //{
            //    product.Unit -= product.Quantity;
            //}

            _productRepository.UpdateRange(products);
            await _productRepository.SaveChangesAsync();

            return order.Id;
        }

        
    }
}
