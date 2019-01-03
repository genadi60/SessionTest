using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace SessionTest.Models
{
    public class Order
    {
        public string Id { get; set; }

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();

        public decimal Total { get; set; }

        public string ClientId { get; set; }
        public virtual IdentityUser Client { get; set; }

        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;

        public string ShippingDataId { get; set; }
        public virtual ShippingData ShippingData { get; set; }
    }
}
