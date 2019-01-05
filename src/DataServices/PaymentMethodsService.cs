using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SessionTest.Common;
using SessionTest.DataServices.Contracts;
using SessionTest.MappingServices;
using SessionTest.Models;
using SessionTest.ViewModels;

namespace SessionTest.DataServices
{
    public class PaymentMethodsService : IPaymentMethodsService
    {
        private readonly IRepository<PaymentMethod> _paymentMethodRepository;

        public PaymentMethodsService(IRepository<PaymentMethod> paymentMethodRepository)
        {
            _paymentMethodRepository = paymentMethodRepository;
        }

        public ICollection<PaymentMethodViewModel> All()
        {
            return _paymentMethodRepository.All().To<PaymentMethodViewModel>().ToList();
        }
    }
}
