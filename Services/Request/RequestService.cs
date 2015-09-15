using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Tumblr.Universal.Services.Request {

    /// <summary>
    /// Provides methods that interface with the Tumblr API.
    /// </summary>
    public class RequestService {

        private static RequestService _requestService;

        /// <summary>
        /// Returns an instace of the TumblrClient object.
        /// </summary>
        public static RequestService Instance {
            get {
                if (_requestService == null)
                    _requestService = new RequestService();
                return _requestService;
            }
        }

        /// <summary>
        /// Primary constructor.
        /// </summary>
        private RequestService() {

        }

        /// <summary>
        /// Performs a request to the Tumblr API to post authentication data.
        /// </summary>
        /// <param name="url">The endpoint to which the request is being made.</param>
        /// <param name="postData">The body of the POST message.</param>
        /// <returns></returns>
        public async Task<string> PostAuthenticationData(string url, string postData) {
            try {
                using (var httpClient = new HttpClient()) {
                    httpClient.MaxResponseContentBufferSize = int.MaxValue;
                    httpClient.DefaultRequestHeaders.ExpectContinue = false;
                    HttpRequestMessage requestMsg = new HttpRequestMessage();

                    requestMsg.Content = new StringContent(postData);
                    requestMsg.Method = new HttpMethod("POST");
                    requestMsg.RequestUri = new Uri(url, UriKind.Absolute);
                    requestMsg.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                    var response = await httpClient.SendAsync(requestMsg);
                    return await response.Content.ReadAsStringAsync();
                }
            } catch (Exception ex) {
                return null;
            }
        }


    }
}
