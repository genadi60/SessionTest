using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SessionTest.Common;
using SessionTest.DataServices.Contracts;
using SessionTest.InputModels;
using SessionTest.MappingServices;
using SessionTest.Models;
using SessionTest.ViewModels;
using SessionExtensions = SessionTest.Common.SessionExtensions;

namespace SessionTest.DataServices
{
    public class PackagesService : IPackagesService
    {
        private readonly IRepository<Package> _packageRepository;
        private readonly IRepository<PaymentMethod> _paymentMethodRepository;
        private readonly IRepository<Cart> _cartRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<CartOrder> _cartOrderRepository;
        private readonly IRepository<ShippingData> _shippingDataRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public PackagesService(IRepository<Package> packageRepository, 
            IRepository<PaymentMethod> paymentMethodRepository, 
            IRepository<Cart> cartRepository, 
            UserManager<IdentityUser> userManager, 
            IRepository<ShippingData> shippingDataRepository, 
            IRepository<Order> orderRepository, 
            IRepository<CartOrder> cartOrderRepository)
        {
            _packageRepository = packageRepository;
            _paymentMethodRepository = paymentMethodRepository;
            _cartRepository = cartRepository;
            _userManager = userManager;
            _shippingDataRepository = shippingDataRepository;
            _orderRepository = orderRepository;
            _cartOrderRepository = cartOrderRepository;
        }

        public PackageInputModel GetPackage(ShippingDataInputModel shipping)
        {
            var cart = _cartRepository.All().To<CartViewModel>().FirstOrDefault(c => c.Id.Equals(shipping.CartId));

            var model = new PackageInputModel
            {
                CartId = cart.Id,
                Cart = cart,
                ShippingData = shipping
            };

            return model;
        }

        
        public async Task<string> ConfirmPackage(HttpContext context, PackageViewModel model)
        {
            var cartId = model.CartId;
            
            var cart = _cartRepository.All().Include(c => c.Orders).FirstOrDefault(c => c.Id.Equals(cartId));
            
            var paymentMethod =
                _paymentMethodRepository.All().FirstOrDefault(pm => pm.Id == int.Parse(model.PaymentMethod));

            var shippingData = Mapper.Map<ShippingData>(model.ShippingData);

            shippingData.Id = Guid.NewGuid().ToString();

            await _shippingDataRepository.AddAsync(shippingData);
            await _shippingDataRepository.SaveChangesAsync();
            
            var package = new Package
            {
                Id = Guid.NewGuid().ToString(),
                PaymentMethod = paymentMethod,
                ShippingData = shippingData,
            };

            var orders = _orderRepository.All().Where(o => o.CartId.Equals(cartId)).ToList();

            foreach (var order in orders)
            {
                order.PackageId = package.Id;
            }

            package.Orders = orders;

            await _packageRepository.AddAsync(package);
            await _packageRepository.SaveChangesAsync();

            _orderRepository.UpdateRange(orders);
            await _orderRepository.SaveChangesAsync();

            var cartOrders = _cartOrderRepository.All().Where(co => co.CartId.Equals(cartId)).ToList();

            _cartOrderRepository.DeleteRange(cartOrders);
            await _cartOrderRepository.SaveChangesAsync();

            _cartRepository.Delete(cart);
            await _cartRepository.SaveChangesAsync();

            SessionExtensions.Set<Cart>(context.Session, cartId, null);

            return package.Id;
        }

        public PackageViewModel GetPackageViewModel(string id)
        {
            var model = _packageRepository.All().To<PackageViewModel>().FirstOrDefault(p => p.Id.Equals(id));

            return model;
        }

        
        public AllPackagesViewModel All()
        {
            var packages = _packageRepository.All().Include(p => p.Orders).To<PackageViewModel>().ToList();

            var model = new AllPackagesViewModel
            {
                Packages = packages
            };

            return model;
        }

        public async Task ConfirmDeliver(string id)
        {
            var package = _packageRepository.All().FirstOrDefault(p => p.Id.Equals(id));

            package.PackageStatus = PackageStatus.Delivered;

            _packageRepository.Update(package);
            await _packageRepository.SaveChangesAsync();
        }

        public async Task ConfirmAcquire(string id)
        {
            var package = _packageRepository.All().FirstOrDefault(p => p.Id.Equals(id));

            package.PackageStatus = PackageStatus.Acquired;

            _packageRepository.Update(package);
            await _packageRepository.SaveChangesAsync();
        }
    }
}
