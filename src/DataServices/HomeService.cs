using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SessionTest.DataServices.Contracts;

namespace SessionTest.DataServices
{
    public class HomeService : IHomeService
    {
        public bool InitialProducts(HttpContext context, string id)
        {
            bool isActive = context.Session.Keys.Any(k => k.Equals(id));
            
            return isActive;
        }
    }
}
