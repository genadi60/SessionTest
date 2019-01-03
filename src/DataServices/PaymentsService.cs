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

        public PaymentMethodInputModel GetMethods()
        {
            var methods = _paymentMethodRepository.All()
                .To<PaymentMethodViewModel>()
                .ToList();

            var model = new PaymentMethodInputModel
            {
                PaymentMethods = methods
            };

            return model;
        }

        [Authorize]
        public bool ConfirmPayment(HttpContext context, PaymentMethodViewModel model, string id)
        {
            Cart cart = SessionExtensions.Get<Cart>(context.Session, id);

            var payment = new Payment
            {
                CartId = cart.Id,
                Cost = cart.Total,
                PaymentMethodId = model.Id,
            };
            return true;
        }
    }
}
