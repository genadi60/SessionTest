using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SessionTest.InputModels;
using SessionTest.ViewModels;

namespace SessionTest.DataServices.Contracts
{
    public interface IPaymentsService
    {
        PaymentMethodInputModel GetMethods();

        bool ConfirmPayment(HttpContext context, PaymentMethodViewModel model, string id);
    }
}
