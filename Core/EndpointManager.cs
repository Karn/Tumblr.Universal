using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tumblr.Universal.Core {

    /// <summary>
    /// Class that manages the Tumblr API's endpoints.
    /// </summary>
    public class EndpointManager {

        //Need a dictionary of documented API endpoints and an alternate list of custom Endpoints 

        private static Dictionary<string, string> _endpoints = new Dictionary<string, string> {
            { "ACCOUNT", "https://api.tumblr.com/v2/user/info" },
            { "DASHBOARD", "/dashboard" },
            //{ "ACTIVITY", "" },
            { "POSTS", "" }
        };

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
