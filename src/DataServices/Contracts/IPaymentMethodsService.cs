using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SessionTest.InputModels;
using SessionTest.ViewModels;

namespace SessionTest.DataServices.Contracts
{
    public interface IPaymentMethodsService
    {
        ICollection<PaymentMethodViewModel> All();
    }
}
