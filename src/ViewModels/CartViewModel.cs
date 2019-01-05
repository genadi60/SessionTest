using System.Collections.Generic;
using System.Linq;
using SessionTest.Models;

namespace SessionTest.ViewModels
{
    public class CartViewModel
    {
        public CartViewModel()
        {
            Orders = new List<OrderViewModel>();
        }

        public string Id { get; set; }

        public bool IsAuthorized { get; set; }

        public virtual ICollection<OrderViewModel> Orders { get; set; }

        public decimal Total => Orders.Sum(o => o.Total);
    }
}
