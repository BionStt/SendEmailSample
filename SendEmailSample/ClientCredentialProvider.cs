// ------------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All Rights Reserved.  Licensed under the MIT License.  See License in the project root for license information.
//

namespace Microsoft.Graph.Auth
{
    using Microsoft.Identity.Client;
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    /// <summary>
    /// An <see cref="IAuthenticationProvider"/> implementation using MSAL.Net to acquire token by client credential flow.
    /// </summary>
    public class ClientCredentialProvider :  IAuthenticationProvider
    {
        private ConfidentialClientApplication ClientApplication;

        private readonly string ResourceUrl = "https://graph.microsoft.com/.default";

        public ClientCredentialProvider(ConfidentialClientApplication confidentialClientApplication)
        {
            ClientApplication = confidentialClientApplication;
        }

        public async Task AuthenticateRequestAsync(HttpRequestMessage httpRequestMessage)
        {
            AuthenticationResult authenticationResult =  await ClientApplication.AcquireTokenForClientAsync(new string[] { ResourceUrl });
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue(CoreConstants.Headers.Bearer, authenticationResult.AccessToken);
        }

        public static ConfidentialClientApplication CreateDaemonApplication(
            string clientId,
            string tenantId,
            string secret
            )
        {
            return new ConfidentialClientApplication(
                        clientId,
                        $"https://login.microsoftonline.com/{tenantId}/",
                        "http://example.org",  // needs to be a URL but not useds
                        new ClientCredential(secret),
                        new TokenCache(),
                        new TokenCache());
        }
    }
}
