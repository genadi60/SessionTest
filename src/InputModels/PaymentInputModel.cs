using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SessionTest.Models;

namespace SessionTest.InputModels
{
    public class PaymentInputModel
    {
        public string Id { get; set; }

        public decimal Cost { get; set; }

        public PaymentMethod PaymentMethod { get; set; }
    }
}
