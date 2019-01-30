using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Microsoft.Graph;
using System.Net.Http.Headers;
using Microsoft.Graph.Auth;

namespace SendEmailSample
{


   class Program
   {
      // App must be registered as "Web" app and have the 
      // ApplicationOnly permissions Mail.Send and Users.Read.All 
        const string tenantId = "07b1effe-c7d9-4825-b95a-06eb0ec626e8";
        const string clientId = "fdc675cd-ffae-434a-912d-2c2fbf3c4693";  
        const string secret = ";/B3GOu/q)V}Eo+@}h{$bBNB*H*=4[91$m{z";

        public async static Task<bool> SendEmailToMyself(string subject, string content)
      {
         string fromUserEmail = "lminkovsky@outlook.com";
         string toUserEmail = "lminkovsky@outlook.com";

         var application = ClientCredentialProvider.CreateDaemonApplication(
                                            clientId,
                                            tenantId,
                                            secret);

            GraphServiceClient graphClient = new GraphServiceClient(
                new ClientCredentialProvider(application));

            var me = await graphClient.Users[fromUserEmail].Request().GetAsync();

            var message = new Message
            {
                Subject = subject,
                Body = new ItemBody() { Content = content },
                ToRecipients = new List<Recipient>()
               {
                   new Recipient()
                   {
                       EmailAddress = new EmailAddress()
                       {
                           Address = toUserEmail
                       }
                   }
               }
            };

            await graphClient.Users[fromUserEmail].SendMail(message, false).Request().PostAsync();
            return true;

      }

      public static async Task TestSendEmailToMyself ()
      {
         bool rc = await SendEmailToMyself(DateTime.Now.ToString(), "This is a test email sent by a sample app");
      }

      static void Main(string[] args)
      {
         TestSendEmailToMyself().Wait();
      }
   }
}
