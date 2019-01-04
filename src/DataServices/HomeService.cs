using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SessionTest.Common;
using SessionTest.Data;
using SessionTest.DataServices.Contracts;
using SessionTest.Models;

namespace SessionTest.DataServices
{
    public class HomeService : IHomeService
    {
        private readonly IRepository<Cart> _cartRepository;

        public HomeService(IRepository<Cart> cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public ICollection<string> CheckSession(HttpContext context)
        {
            var cacheCartsId = _cartRepository.All().Select(c => c.Id).ToList();

            var inactiveCartsId = new List<string>();

            foreach (var id in cacheCartsId)
            {
                if (!context.Session.Keys.Any(k => k.Equals(id)))
                {
                    inactiveCartsId.Add(id);
                }
            }

            return inactiveCartsId;
        }
    }
}
