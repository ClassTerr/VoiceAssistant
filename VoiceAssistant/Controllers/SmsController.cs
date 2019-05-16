using Twilio.AspNet.Common;
using Twilio.AspNet.Core;
using Twilio.TwiML;

namespace VoiceAssistant.Controllers
{
    public class SmsController : TwilioController
    {
        // GET: Sms
        public TwiMLResult Index(SmsRequest request)
        {
            var response = new MessagingResponse();
            response.Message(
                $"Hey there {request.From}! " +
                "How 'bout those Seahawks?"
            );
            return TwiML(response);
        }
    }
}
