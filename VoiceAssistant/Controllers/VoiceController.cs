using System;
using System.Collections.Generic;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Twilio.AspNet.Common;
using Twilio.AspNet.Core;
using Twilio.TwiML;
using Twilio.TwiML.Voice;
using Google.Cloud.Dialogflow.V2;
using Grpc.Auth;
using Grpc.Core;
using System.Threading.Tasks;

// ReSharper disable StringLiteralTypo

namespace VoiceAssistant.Controllers
{
    public class VoiceController : TwilioController
    {
        public IConfiguration Configuration { get; }

        public VoiceController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [HttpPost]
        public async Task<TwiMLResult> Index(VoiceRequest request)
        {
            Console.WriteLine(request.SpeechResult);
            VoiceResponse response = new VoiceResponse();
            Gather gather = new Gather(new List<Gather.InputEnum> { Gather.InputEnum.Speech },
                language: Gather.LanguageEnum.RuRu);



            
            if (String.IsNullOrEmpty(request.SpeechResult))
            {
                gather.Say(
                    "Привет. Я голосовой ассистент. Всё что ты скажешь дальше я повторю за тобой.",
                    language: Say.LanguageEnum.RuRu);
            }
            else
            {
                String dfRes = await AskDialogflow(request.SpeechResult, request.SipCallId);
                gather.Say(dfRes, language: Say.LanguageEnum.RuRu);
            }


            response.Append(gather);
            return TwiML(response);
        }

        public async Task<String> AskDialogflow(String s, String dialogIdentifier)
        {
            if (s == null) throw new ArgumentNullException(nameof(s));

            QueryInput query = new QueryInput
            {
                Text = new TextInput
                {
                    Text = s,
                    LanguageCode = "ru-ru"
                }
            };

            String sessionId = dialogIdentifier ?? Guid.NewGuid().ToString();
            String agent = "voiceassistant-ba8d7";
            GoogleCredential credentials = GoogleCredential.FromJson(Configuration["GoogleAuth"]);
            Channel channel = new Channel(SessionsClient.DefaultEndpoint.Host, credentials.ToChannelCredentials());

            SessionsClient client = SessionsClient.Create(channel);

            DetectIntentResponse dialogFlow = client.DetectIntent(
                new SessionName(agent, sessionId),
                query
            );

            await channel.ShutdownAsync().ConfigureAwait(false);

            return dialogFlow.QueryResult.FulfillmentText;
        }
    }
}