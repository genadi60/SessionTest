using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SessionTest.MappingServices;
using SessionTest.Models;

namespace SessionTest.ViewModels
{
    public class PaymentMethodViewModel : IMapFrom<PaymentMethod>
    {

        public int Id { get; set; }

        public string Name { get; set; }
    }
}
