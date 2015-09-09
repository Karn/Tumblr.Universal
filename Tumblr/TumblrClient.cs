using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TumblrUniversal.Tumblr {

    /// <summary>
    /// Global Tumblr Library object.
    /// </summary>
    public class TumblrClient {

        internal static string API_KEY { get; private set; }

        internal static string SECRET_KEY { get; private set; }

        private static TumblrClient _authenticationService;

        /// <summary>
        /// Returns an instace of the TumblrClient object.
        /// </summary>
        public static TumblrClient Instance {
            get {
                if (_authenticationService == null)
                    _authenticationService = new TumblrClient();
                return _authenticationService;
            }
        }

        /// <summary>
        /// Primary constructor.
        /// </summary>
        private TumblrClient() {

        }

        /// <summary>
        /// Registers API keys to grant access to the API endpoints.
        /// </summary>
        /// <param name="publicKey">The API/consumer key.</param>
        /// <param name="secretKey">The associated client secret key.</param>
        public static void RegisterAPIKeys(string publicKey, string secretKey) {
            if (!string.IsNullOrWhiteSpace(publicKey) && !string.IsNullOrWhiteSpace(secretKey)) {
                API_KEY = publicKey;
                SECRET_KEY = secretKey;
            } else {
                throw new Exception("One or both of the parameters is null or an empty string.");
            }
        }

        public string RetrieveRawResponse(string endpoint) {

            return null;
        }
    }
}
