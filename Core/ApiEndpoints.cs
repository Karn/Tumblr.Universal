using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tumblr.Universal.Core;

namespace Tumblr.Universal.Core {

    /// <summary>
    /// Defines the basic endpoints for the Tumblr API.
    /// </summary>
    public class ApiEndpoints {

        /// <summary>
        /// The protocol from which an API request will be made.
        /// </summary>
        public const string PROTOCOL = "https";

        /// <summary>
        /// The base url to the API.
        /// </summary>
        public const string BASE_URL = "api.tumblr.com/v2";

        /// <summary>
        /// An endpoint struct that produces a complete url to the requested endpoint
        /// via it's toString() method.
        /// </summary>
        public struct Endpoint {
            private string URL { get; set; }

            /// <summary>
            /// Constructor that assigns the value for the endpoint.
            /// </summary>
            /// <param name="URL">A string representing the endpoint without the 
            /// BASE_URL or PROTOCOL.</param>
            public Endpoint(string URL) {
                this.URL = URL;
            }

            /// <summary>
            /// Returns a complete url to the requested endpoint.
            /// </summary>
            /// <returns></returns>
            public override string ToString() {
                return string.Format("{0}://{1}{2}", PROTOCOL, BASE_URL, this.URL);
            }
        }

        public static Endpoint ACCOUNT = new Endpoint("/user/info");
        public static Endpoint DASHBOARD = new Endpoint("/dashboard");
        public static Endpoint BLOG = new Endpoint("/blog/{0}.tumblr.com/info");
        public static Endpoint BLOG_POSTS = new Endpoint("/blog/{0}.tumblr.com/posts");
        public static Endpoint ACTIVITY = new Endpoint("");
        public static Endpoint POSTS = new Endpoint("");
        public static Endpoint LIKE = new Endpoint("/user/like");
        public static Endpoint UNLIKE = new Endpoint("/user/unlike");
        public static Endpoint REBLOG = new Endpoint("/blog/{0}.tumblr.com/post/reblog");
    }
}
