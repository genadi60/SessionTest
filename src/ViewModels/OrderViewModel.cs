using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SessionTest.Models;

namespace SessionTest.ViewModels
{
    public class OrderViewModel
    {
        public string Id { get; set; }

        public string ProductId { get; set; }
        public virtual Product Product{ get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal Total => Quantity * Price;
    }
}
