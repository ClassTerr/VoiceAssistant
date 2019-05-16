using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Twilio.Rest.Api.V2010.Account;
using VoiceAssistant.Models;

namespace VoiceAssistant.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
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

        [HttpPost]
        public IActionResult Call([FromBody]String number)
        {

            CallResource call = CallResource.Create(
                
                to: new Twilio.Types.PhoneNumber(number),
                from: new Twilio.Types.PhoneNumber("+18647611606")
            );

            return Ok(call.Sid);
        }
    }
}
