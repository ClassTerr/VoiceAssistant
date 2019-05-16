using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Twilio.Jwt;
using Twilio.Jwt.Client;

namespace VoiceAssistant.Controllers
{
    public class TokenController : Controller
    {
        public IConfiguration Configuration { get; }

        public TokenController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [HttpPost]
        public ActionResult Index()
        {
            String accountSid = Configuration["Twilio:AccountSID"];
            String authToken = Configuration["Twilio:AuthToken"];

            HashSet<IScope> scopes = new HashSet<IScope>
            {
                new IncomingClientScope("joey")
            };
            ClientCapability capability = new ClientCapability(accountSid, authToken, scopes: scopes);

            return Content(capability.ToJwt(), "application/jwt");
        }
    }
}