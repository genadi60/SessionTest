using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SessionTest.Models;
using SessionTest.ViewModels;

namespace SessionTest.InputModels
{
    public class PackageInputModel
    {
        public PackageInputModel()
        {
            
        }

        public string Id { get; set; }

        public decimal Amount => Cart.Total;

        public string CartId { get; set; }
        public virtual CartViewModel Cart { get; set; }

        public string ShippingDataId { get; set; }
        public virtual ShippingDataInputModel ShippingData { get; set; }

        public string PaymentMethod { get; set; }
    }
}
