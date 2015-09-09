using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TumblrUniversal.Services {

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
    }
}
