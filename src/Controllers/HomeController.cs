using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using SessionTest.DataServices.Contracts;
using SessionTest.Models;
using SessionTest.ViewModels;

namespace SessionTest.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IHomeService _homeService;
        private readonly IOrdersService _ordersService;
        private readonly IProductsService _productsService;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        
        public HomeController(IHomeService homeService, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IOrdersService ordersService, IProductsService productsService)
        {
            _homeService = homeService;
            _signInManager = signInManager;
            _userManager = userManager;
            _ordersService = ordersService;
            _productsService = productsService;
        }

        public IActionResult Index(IndexViewModel model)
        {
            var inactiveSessions =_homeService.InitialSession(HttpContext);


            if (inactiveSessions.Any())
            {
                _ordersService.InitialDatabase(inactiveSessions);
            }

            var products = _productsService.GetAll<ProductViewModel>();

            model.Products = products;

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
    }
}
