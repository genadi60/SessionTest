using System.Collections.Generic;
using System.Linq;
using SessionTest.Models;

namespace SessionTest.ViewModels
{
    public class CartViewModel
    {
        public CartViewModel()
        {
            Orders = new List<Order>();
        }

        public string Id { get; set; }

        public ICollection<Order> Orders { get; set; }

        public decimal Total => Orders.Sum(o => o.Total);
    }
}
