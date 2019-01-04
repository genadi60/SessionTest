using SessionTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SessionTest.ViewModels
{
    public class PaymentViewModel
    {
        public string Id { get; set; }

        public decimal Cost => Cart.Total;

        public string CartId { get; set; }
        public virtual Cart Cart { get; set; }

        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Active;

        public int PaymentMethodId { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }

        public string ShippingDataId { get; set; }
        public virtual ShippingData ShippingData { get; set; }

        public DateTime PaidOn { get; set; } = DateTime.UtcNow;
    }
}
