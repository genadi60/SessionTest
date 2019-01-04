using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SessionTest.Models;

namespace SessionTest.InputModels
{
    public class PaymentInputModel
    {
        public string Id { get; set; }

        public decimal Cost => Cart.Total;

        public string CartId { get; set; }
        public virtual Cart Cart { get; set; }

        public string ShippingDataId { get; set; }
        public virtual ShippingDataInputModel ShippingData { get; set; }

        public ICollection<PaymentMethodInputModel> PaymentMethods { get; set; }
    }
}
