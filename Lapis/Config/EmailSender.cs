using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace Lapis.Config
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return SendEmail(email , subject , htmlMessage);
        }
        public async Task SendEmail(string email, string subject, string htmlMessage)
        { 
            MailjetClient client = new MailjetClient("22f6cfded3d579f07fc092e822b36380", "0071437aa97edc757229cd64a8ef8418")
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
        {"Email", "ammar.m777.2@gmail.com"},
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
