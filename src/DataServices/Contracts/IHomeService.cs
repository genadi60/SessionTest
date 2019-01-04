using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SessionTest.DataServices.Contracts
{
    public interface IHomeService
    {
        ICollection<string> CheckSession(HttpContext context);
    }
}
