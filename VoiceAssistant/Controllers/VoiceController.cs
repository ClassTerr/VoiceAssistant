using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Twilio.AspNet.Common;
using Twilio.AspNet.Core;
using Twilio.TwiML;
using Twilio.TwiML.Voice;

// ReSharper disable StringLiteralTypo

namespace VoiceAssistant.Controllers
{
    public class VoiceController : TwilioController
    {
        [HttpPost]
        public TwiMLResult Index(VoiceRequest request)
        {
            Console.WriteLine(request.SpeechResult);
            VoiceResponse response = new VoiceResponse();
            Gather gather = new Gather(new List<Gather.InputEnum> {Gather.InputEnum.Speech},
                language: Gather.LanguageEnum.RuRu);

            if (String.IsNullOrEmpty(request.SpeechResult))
                gather.Say(
                    "Привет. Я голосовой ассистент. Всё что ты скажешь дальше я повторю за тобой.",
                    language: Say.LanguageEnum.RuRu);
            else
                gather.Say("Ты сказал: " + request.SpeechResult + "Говори дальше!",
                    language: Say.LanguageEnum.RuRu);


            response.Append(gather);
            return TwiML(response);
        }
    }
}