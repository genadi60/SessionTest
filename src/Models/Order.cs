using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace SessionTest.Models
{
    public class Order
    {
        public string Id { get; set; }

        public string ProductId { get; set; }
        public virtual Product Product{ get; set; }

        public string CartId { get; set; }
        public virtual Cart Cart { get; set; }

        public string PackageId { get; set; }
        public virtual Package Package { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal Total => Quantity * Price;

    }
}
