using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SessionTest.ViewModels
{
    public class CartProductViewModel
    {
        public string Id { get; set; }

        public string CartId { get; set; }
        public virtual CartViewModel Cart { get; set; }

        public string ProductId { get; set; }
        public virtual ProductViewModel Product { get; set; }
    }
}
