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
        private readonly IProductsService _productsService;
        private readonly IHomeService _homeService;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(IProductsService productsService, IHomeService homeService, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _productsService = productsService;
            _homeService = homeService;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Index(IndexViewModel model)
        {
            //var message = new MimeMessage();
            //message.From.Add(new MailboxAddress(Encoding.UTF8, "SessionTest", "genadimihaylov@gmail.com"));
            //message.To.Add(new MailboxAddress(Encoding.UTF8, "gena", "genadi60@abv.bg"));
            //message.Subject = "testing mail in asp.net core";
            //message.Body =  new TextPart("plain")
            //{
            //    Text = "Здравей, честита нова година!"
            //};
            //var message = new MailMessage();
            //message.From = new MailAddress("genadimihaylov@gmail.com");
            //message.To.Add("genadi60@abv.bg");
            //message.Body = "Здравей, честита новата 2019 година!";
            //message.Subject = "testing mail in asp.net core";

            //using (var client = new SmtpClient("mysmtpserver"))
            //{
            //    client.UseDefaultCredentials = false;
            //    client.Credentials = new NetworkCredential("username", "password");
                
            //    //client.Connect("smtp.gmail.com", 587, false);
            //    //client.Authenticate("genadimihaylov@gmail.com", "Mariyana");
            //    client.Send(message);
            //    client.Dispose();//.Disconnect(true);
            //}
            if (_signInManager.IsSignedIn(User))
            {
                string id = _userManager.GetUserId(User);

                var isActiveSession = _homeService.InitialProducts(HttpContext, id);

                if (!isActiveSession)
                {
                    _productsService.InitialProducts();
                }
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
