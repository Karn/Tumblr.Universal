using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tumblr.Universal.Services;
using Tumblr.Universal.Services.Request;

namespace Tumblr.Universal.Core {

    /// <summary>
    /// Global Tumblr Library object.
    /// </summary>
    public class TumblrClient {

        //OAuth URI Endpoints
        internal static readonly string RequestTokenURI = "http://www.tumblr.com/oauth/request_token";
        internal static readonly string AuthorizationURI = "http://www.tumblr.com/oauth/authorize";
        internal static readonly string AccessTokenURI = "http://www.tumblr.com/oauth/access_token";
        //XAuth URI Endpoints
        internal static readonly string SecureAccessTokenURI = "https://www.tumblr.com/oauth/access_token";

        /// <summary>
        /// Returns a publicly accessible instance of the AuthenticationService object.
        /// </summary>
        public AuthenticationService Authentication { get; private set; }

        /// <summary>
        /// Returns a publicly accessible instance of the RequestService object.
        /// </summary>
        public RequestService Requests { get; private set; }

        internal static string ConsumerKey { get; private set; }

        internal static string ConsumerSecretKey { get; private set; }

        internal static string AccessToken { get; private set; }

        internal static string AccessSecretToken { get; private set; }

        /// <summary>
        /// Returns true if there are access tokens associated with the current session.
        /// </summary>
        public bool SignedIn {
            get { return !string.IsNullOrWhiteSpace(AccessToken) && !string.IsNullOrWhiteSpace(AccessSecretToken); }
        }

        /// <summary>
        /// Registers API keys to grant access to the API endpoints.
        /// </summary>
        /// <param name="publicKey">The API/consumer key.</param>
        /// <param name="secretKey">The associated client secret key.</param>
        public TumblrClient(string publicKey, string secretKey) {
            if (!string.IsNullOrWhiteSpace(publicKey) && !string.IsNullOrWhiteSpace(secretKey)) {
                ConsumerKey = publicKey;
                ConsumerSecretKey = secretKey;

                Authentication = AuthenticationService.Instance;
                Requests = RequestService.Instance;
            } else {
                throw new Exception("One or both of the parameters is null or an empty string.");
            }
        }

        /// <summary>
        /// Registers tokens recieved from authenticated request.
        /// </summary>
        /// <param name="token">The authenticated token.</param>
        /// <param name="tokenSecret">The associated secret token.</param>
        public static void UpdateAccessToken(string token, string tokenSecret) {
            if (!string.IsNullOrWhiteSpace(token) && !string.IsNullOrWhiteSpace(tokenSecret)) {
                AccessToken = token;
                AccessSecretToken = tokenSecret;
            } else {
                throw new Exception("One or both of the parameters is null or an empty string.");
            }
        }

        /// <summary>
        /// Returns the access token associated with the current session.
        /// </summary>
        public string GetAccessToken {
            get { return AccessToken; }
        }

        /// <summary>
        /// Returns the access secret token associated with the current session.
        /// </summary>
        public string GetAccessSecretToken {
            get { return AccessSecretToken; }
        }
    }
}
