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
        PaymentInputModel GetPayment(ShippingDataInputModel shipping);

        bool ConfirmPayment(HttpContext context, PaymentViewModel model, string id);
    }
}
