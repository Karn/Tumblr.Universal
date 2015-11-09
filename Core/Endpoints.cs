using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tumblr.Universal.Core;

namespace Tumblr.Universal.Core {

    public class APIEndpoints {

        /// <summary>
        /// The protocol from which an API request will be made.
        /// </summary>
        public const string PROTOCOL = "https";

        /// <summary>
        /// The base url to the API.
        /// </summary>
        public const string BASE_URL = "api.tumblr.com/v2";

        public struct Endpoint {
            private string URL { get; set; }

            public Endpoint(string URL) {
                this.URL = URL;
            }

            public override string ToString() {
                return string.Format("{0}://{1}{2}", APIEndpoints.PROTOCOL, APIEndpoints.BASE_URL, this.URL);
            }
        }

        public static Endpoint ACCOUNT = new Endpoint("/user/info");
        public static Endpoint DASHBOARD = new Endpoint("/dashboard");
        public static Endpoint ACTIVITY = new Endpoint("");
        public static Endpoint POSTS = new Endpoint("");
        public static Endpoint LIKE = new Endpoint("/user/like");
        public static Endpoint UNLIKE = new Endpoint("/user/unlike");
        public static Endpoint REBLOG = new Endpoint("/blog/{0}.tumblr.com/post/reblog");


        public APIEndpoints() {

        }
    }

    /// <summary>
    /// Class that manages the Tumblr API's endpoints.
    /// </summary>
    public class CustomEndpoints : Dictionary<string, string> {

        private static Dictionary<string, string> _endpoints = new Dictionary<string, string> {
            { "ACCOUNT", "user/info" },
            { "DASHBOARD", "/dashboard" },
            { "ACTIVITY", "" },
            { "POSTS", "" },
            { "LIKE", "/user/like" },
            { "UNLIKE", "/user/unlike" }
        };

        /// <summary>
        /// Hides the IDictonary this[TKey Key], opting to return a modified version of the value
        /// that is represented by a given key.
        /// In this instance, the value is modified to return a URL endpoint.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public new string this[string key] {
            get {
                return string.Format("{0}://{1}{2}", APIEndpoints.PROTOCOL, APIEndpoints.BASE_URL, _endpoints[key]);
            }
            set {
                _endpoints[key] = value;
            }
        }

        //Need a dictionary of documented API endpoints and an alternate list of custom Endpoints 
        private static CustomEndpoints _serviceEndpoints;

        /// <summary>
        /// Returns an instace of the TumblrClient object.
        /// </summary>
        public static CustomEndpoints Instance {
            get {
                if (_serviceEndpoints == null)
                    _serviceEndpoints = new CustomEndpoints();
                return _serviceEndpoints;
            }
        }

        /// <summary>
        /// Primary constructor.
        /// </summary>
        private CustomEndpoints() {

        }

        /// <summary>
        /// Return a dictonary containing documented endpoints.
        /// </summary>
        public static Dictionary<string, string> EndPoints {
            get {
                return _endpoints;
            }
        }

        /// <summary>
        /// Allows for the addition of a undocumented/custom endpoint.
        /// </summary>
        /// <param name="identifier">A short name of the endpoint. Must be uppercase. Eg. 'DASHBOARD'.</param>
        /// <param name="endpoint">The endpoint (not including the base hostname). Eg. '/dashboard'.</param>
        public static void AddEndpoint(string identifier, string endpoint) {
            if (_endpoints.Keys.Contains(identifier.ToUpper())) {
                throw new Exception("The identifier defined already exists.");
            } else if (_endpoints.Values.Contains(endpoint)) {
                throw new Exception("The endpoint defined already exists");
            } else {
                _endpoints.Add(identifier.ToUpper(), endpoint);
            }
        }
    }
}
