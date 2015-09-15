using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tumblr.Universal.Services.Request;
using Tumblr.Universal.Core;

namespace Tumblr.Universal.Services {

    /// <summary>
    /// Class that handles account authentication via xAuth.
    /// </summary>
    public class AuthenticationService {

        //OAuth URI Endpoints
        public static readonly string RequestTokenURI = "http://www.tumblr.com/oauth/request_token";
        public static readonly string AuthorizationURI = "http://www.tumblr.com/oauth/authorize";
        public static readonly string AccessTokenURI = "http://www.tumblr.com/oauth/access_token";
        //XAuth URI Endpoints
        public static readonly string SecureAccessTokenURI = "https://www.tumblr.com/oauth/access_token";

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

            var signatureString = "POST&" + Uri.EscapeDataString(SecureAccessTokenURI) + "&" +
                Uri.EscapeDataString(signatureParameters);
            var signature = RequestBuilder.Instance.GenerateSignature(signatureString, "auth");

            var response = await RequestService.Instance.PostAuthenticationData(SecureAccessTokenURI,
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
