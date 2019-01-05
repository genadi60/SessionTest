using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using SessionTest.ViewModels;

namespace SessionTest.Models
{
    public class Cart
    {
        public Cart()
        {
            Orders = new List<Order>();
        }

        public string Id { get; set; }

        public bool IsAuthorized { get; set; } = false;

        public decimal Total => Orders.Sum(o => o.Total);

        public virtual ICollection<Order> Orders { get; set; }
    }
}
