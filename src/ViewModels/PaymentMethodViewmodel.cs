using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SessionTest.ViewModels
{
    public class PaymentMethodViewModel
    {
        public int Id { get; set; }

        public string CartId { get; set; }

        public decimal Total { get; set; }

        public string Name { get; set; }
    }
}
