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

        const string tenantId = "f8cdef31-a31e-4b4a-93e4-5f571e91255a";
        const string clientId = "aee626c6-7252-4824-9923-6a9aea0d4591";  
        const string secret = "mR4&s3:Ysd.fOdb)%";

        public async static Task<bool> SendEmailToMyself(string subject, string content)
      {
         string fromUserEmail = "<from email here>";
         string toUserEmail = "<to email here>";

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
