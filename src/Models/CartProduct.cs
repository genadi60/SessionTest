using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SessionTest.Models
{
    public class CartProduct
    {
        public string Id { get; set; }

        public string CartId { get; set; }
        public virtual Cart Cart { get; set; }

        public string ProductId { get; set; }
        public virtual Product Product { get; set; }

    }
}
