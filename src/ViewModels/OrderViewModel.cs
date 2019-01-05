using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SessionTest.InputModels;
using SessionTest.Models;

namespace SessionTest.ViewModels
{
    public class OrderViewModel
    {
        public string Id { get; set; }

        public string ProductId { get; set; }

        public virtual ProductInputModel Product{ get; set; }

        public int Quantity { get; set; }

        public decimal Total => Quantity * Product.Price;
    }
}
