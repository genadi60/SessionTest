using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using SessionTest.Common;
using SessionTest.DataServices.Contracts;
using SessionTest.InputModels;
using SessionTest.MappingServices;
using SessionTest.Models;
using SessionTest.ViewModels;
using SessionExtensions = SessionTest.Common.SessionExtensions;

namespace SessionTest.DataServices
{
    public class PaymentsService : IPaymentsService
    {
        private readonly IRepository<Payment> _paymentRepository;
        private readonly IRepository<PaymentMethod> _paymentMethodRepository;

        public PaymentsService(IRepository<Payment> paymentRepository, IRepository<PaymentMethod> paymentMethodRepository)
        {
            _paymentRepository = paymentRepository;
            _paymentMethodRepository = paymentMethodRepository;
        }

        public PaymentInputModel GetPayment(ShippingDataInputModel shipping)
        {
            var methods = _paymentMethodRepository.All()
                .To<PaymentMethodInputModel>()
                .ToList();

            var model = new PaymentInputModel
            {
                PaymentMethods = methods
            };

            return model;
        }

        
        public bool ConfirmPayment(HttpContext context, PaymentViewModel model, string id)
        {
            Cart cart = SessionExtensions.Get<Cart>(context.Session, id);

            var payment = new Payment
            {
                CartId = cart.Id,
            };
            return true;
        }
    }
}
