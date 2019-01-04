using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SessionTest.Models;
using SessionTest.ViewModels;

namespace SessionTest.InputModels
{
    public class PaymentMethodInputModel
    {
        public PaymentMethodInputModel()
        {
            PaymentMethods = new List<PaymentMethodViewModel>();
        }

        public string Name { get; set; }
        public string CartId { get; set; }
        public virtual ICollection<PaymentMethodViewModel> PaymentMethods { get; set; }
    }
}
