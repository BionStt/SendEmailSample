using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Microsoft.Graph;
using System.Net.Http.Headers;

namespace SendEmailSample
{
   class Program
   {
      public async static Task<AuthenticationResult> SilentyAuthenticate()
      {
         //TODO - implement proper authenication procedure with Microsoft.Graph.Auth
         return null;
      }

      public async static Task<bool> SendEmailToMyself(string subject, string content)
      {
         AuthenticationResult authResult = await SilentyAuthenticate();

         if (authResult != null)
         {
            GraphServiceClient graphClient = new GraphServiceClient(
                new DelegateAuthenticationProvider(
                    (requestMessage) =>
                    {
                       requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", authResult.AccessToken);
                       return Task.FromResult(0);
                    }));

            var me = await graphClient.Me.Request().GetAsync();

            var message = new Message();
            message.Subject = subject;
            message.Body = new ItemBody() { Content = content };
            var recipients = new List<Recipient>()
               {
                   new Recipient()
                   {
                       EmailAddress = new EmailAddress()
                       {
                           Address = me.UserPrincipalName.ToString()
                       }
                   }
               };

            message.ToRecipients = recipients;

            await graphClient.Me.SendMail(message, false).Request().PostAsync();
            return true;
         }

         return false;

      }

      public static async void TestSendEmailToMyself ()
      {
         bool rc = await SendEmailToMyself(DateTime.Now.ToString(), "This is a test email to myself sent by a sample app");
      }

      static void Main(string[] args)
      {
         TestSendEmailToMyself();
      }
   }
}
