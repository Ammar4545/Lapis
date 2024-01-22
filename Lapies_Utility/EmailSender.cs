using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace Lapis_Utility
{
    public class EmailSender : IEmailSender
    {
        public readonly IConfiguration _configuration;
        public EmailSender(IConfiguration configuration )
        {
            _configuration = configuration;
        }
        public MailJet mailJet { get; set; }
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return SendEmail(email , subject , htmlMessage);
        }
        public async Task SendEmail(string email, string subject, string htmlMessage)
        {
            mailJet = _configuration.GetSection("MailJet").Get<MailJet>();
            MailjetClient client = new MailjetClient(mailJet.ApiKey, mailJet.SecretKey)
            {
                Version = ApiVersion.V3_1,
            };
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
             .Property(Send.Messages, new JArray {
     new JObject {
      {
       "From",
       new JObject {
        {"Email", "testlapies@proton.me"},
        {"Name", "ammar"}
       }
      }, {
       "To",
       new JArray {
        new JObject {
         {
          "Email",
          email
         }, {
          "Name",
          "Testing"
         }
        }
       }
      }, {
       "Subject",
       subject  
      }, {
       "HTMLPart",
       htmlMessage
      }, {
       "CustomID",
       "AppGettingStartedTest"
      }
     }
             });
             await client.PostAsync(request);
        }
    }
}
