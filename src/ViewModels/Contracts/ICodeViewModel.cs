using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SessionTest.ViewModels.Contracts
{
    public interface ICodeViewModel
    {
        string Id { get; set; }

        bool IsAuthorized { get; set; }

        string Guest { get; set; }

        string Message { get; set; }
    }
}
