using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tumblr.Universal.Core;
using Tumblr.Universal.Services.Request;

namespace Tumblr.Universal.Services {

    /// <summary>
    /// Class that handles account authentication via xAuth.
    /// </summary>
    public class AuthenticationService {

        private static AuthenticationService _authenticationService;

        /// <summary>
        /// Returns an instace of the AuthenticationService object.
        /// </summary>
        internal static AuthenticationService Instance {
            get {
                if (_authenticationService == null)
                    _authenticationService = new AuthenticationService();
                return _authenticationService;
            }
        }

        /// <summary>
        /// Primary constructor.
        /// </summary>
        private AuthenticationService() {

        }

        /// <summary>
        /// Makes a request to retrieve an authenticated access token using OAuth 2.0.
        /// </summary>
        /// <param name="userName">The username(email) of the Tumblr account.</param>
        /// <param name="password">The password of the Tumblr account.</param>
        /// <returns></returns>
        public async Task RequestAccessToken(string userName, string password) {
            var nonce = RequestBuilder.Instance.GetNonce();
            var timeStamp = RequestBuilder.Instance.GetTimeStamp();

            var signatureParameters = new SortedDictionary<string, string> {
                { "oauth_consumer_key", TumblrClient.ConsumerKey},
                {"oauth_nonce", nonce},
                {"oauth_signature_method", "HMAC-SHA1"},
                {"oauth_timestamp", timeStamp},
                {"oauth_version", "1.0"},
                {"x_auth_mode", "client_auth"},
                {"x_auth_password", RequestBuilder.Instance.Encode(password)},
                {"x_auth_username", RequestBuilder.Instance.Encode(userName)}}
            .Select(kv => kv.Key + "=" + kv.Value).Aggregate((i, j) => i + "&" + j);

            var signatureString = "POST&" + Uri.EscapeDataString(TumblrClient.SecureAccessTokenURI) + "&" +
                Uri.EscapeDataString(signatureParameters);
            var signature = RequestBuilder.Instance.GenerateSignature(signatureString, "auth");

            var response = await RequestBuilder.Instance.PostAuthenticationData(TumblrClient.SecureAccessTokenURI,
                signatureParameters + "&oauth_signature=" + Uri.EscapeDataString(signature));
            
            //Parse response data
            if (!string.IsNullOrEmpty(response) && response.Contains("oauth_token") && response.Contains("oauth_token_secret")) {
                var tokens = response.Split('&');
                var accessToken = tokens[0].Split('=');
                var accessTokenSecret = tokens[1].Split('=');
                
                try {
                    TumblrClient.UpdateAccessToken(accessToken[1], accessTokenSecret[1]);
                } catch (Exception e) {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    throw new Exception("There was an error parsing the response.");
                }
            } else if (response == "Invalid xAuth credentials.") {
                throw new Exception(response);
            }
        }
    }
}
