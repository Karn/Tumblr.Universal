using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Tumblr.Universal.Core;
using Tumblr.Universal.Core.Entities;

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
        /// Provides an accessor for the global URL encoder.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string Encode(string s) {
            return RequestBuilder.Instance.Encode(s);
        }


        /// <summary>
        /// Fetches the account information of the blogs associated with the current session.
        /// </summary>
        /// <returns>Parsed JSON as an 'Account' object.</returns>
        public async Task<Account> RetrieveAccount() {
            var result = await RequestBuilder.Instance.GET(APIEndpoints.ACCOUNT.ToString(), new RequestParameters());

            if (result.StatusCode == HttpStatusCode.OK) {
                try {
                    Debug.WriteLine(await result.Content.ReadAsStringAsync());
                    var parsedData = JsonConvert.DeserializeObject<ResponseModel.GetInfo>(await result.Content.ReadAsStringAsync());

                    return parsedData.response.user;
                } catch {
                    throw new Exception("Unable to deserialize response into JSON object.");
                }
            }

            throw new Exception(string.Format("Request failed, server returned '{0}' with reason '{1}'", result.StatusCode, result.ReasonPhrase));
        }

        /// <summary>
        /// Retrieves the activity feed of the selected account.
        /// </summary>
        /// <param name="blogName"></param>
        /// <returns>List of 'ActivityItem' objects.</returns>
        public async Task<List<ActivityItem>> RetrieveActivity(string blogName) {
            var result = await RequestBuilder.Instance.GET("https://api.tumblr.com/v2/blog/" + blogName + ".tumblr.com/notifications",
                    new RequestParameters() {
                        { "rfg", "true" }
                    });

            if (result.StatusCode == HttpStatusCode.OK) {
                try {
                    Debug.WriteLine(await result.Content.ReadAsStringAsync());
                    var parsedData = JsonConvert.DeserializeObject<ResponseModel.GetActivity>(await result.Content.ReadAsStringAsync());

                    foreach (var item in parsedData.response.notifications) {
                        item.Date = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(item.Timestamp).Date;
                    }

                    return parsedData.response.notifications;
                } catch {
                    throw new Exception("Unable to deserialize response into JSON object.");
                }
            }

            throw new Exception(string.Format("Request failed, server returned '{0}' with reason '{1}'", result.StatusCode, result.ReasonPhrase));
        }

        /// <summary>
        /// Retrieves posts from a given API endpoint.
        /// </summary>
        /// <param name="endPoint">The endpoint from which the requests are to be retrieved from. Eg. /dashboard</param>
        /// <param name="parameters">List of parameters that are to be included in the request. Eg. Key: "reblog_info" Value: "true"</param>
        /// <returns>List of 'ActivityItem' objects.</returns>
        public async Task<List<PostItem>> RetrievePosts(string endPoint, RequestParameters parameters) {
            parameters.Add("api_key", TumblrClient.ConsumerKey);
            parameters.Add("reblog_info", "true");

            var result = await RequestBuilder.Instance.GET(endPoint, parameters);

            if (result.StatusCode == HttpStatusCode.OK) {
                try {
                    Debug.WriteLine(await result.Content.ReadAsStringAsync());
                    var parsedData = JsonConvert.DeserializeObject<ResponseModel.GetPosts>(await result.Content.ReadAsStringAsync());

                    return parsedData.response.posts;
                } catch {
                    throw new Exception("Unable to deserialize response into JSON object.");
                }
            }

            throw new Exception(string.Format("Request failed, server returned '{0}' with reason '{1}'", result.StatusCode, result.ReasonPhrase));
        }

        /// <summary>
        /// Creates a GET request to 'like' a given post.
        /// </summary>
        /// <param name="id">The id of the given post.</param>
        /// <param name="reblogKey">The corresponsing reblogKey for the given post.</param>
        /// <returns></returns>
        public async Task<bool> LikePost(string id, string reblogKey) {
            return (await RequestBuilder.Instance.GET(APIEndpoints.LIKE.ToString(),
                new RequestParameters() {
                    {"id", id },
                    {"reblog_key", reblogKey}
                })).StatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// Creates a GET request to 'unlike' a given post.
        /// </summary>
        /// <param name="id">The id of the given post.</param>
        /// <param name="reblogKey">The corresponsing reblogKey for the given post.</param>
        /// <returns></returns>
        public async Task<bool> UnlikePost(string id, string reblogKey) {
            return (await RequestBuilder.Instance.GET(APIEndpoints.UNLIKE.ToString(),
                new RequestParameters() {
                    {"id", id },
                    {"reblog_key", reblogKey}
                })).StatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// Creates a POST request to 'reblog' a given post.
        /// </summary>
        /// <param name="id">The id of the given post.</param>
        /// <param name="reblogKey">The corresponding reblogKey for the given post.</param>
        /// <param name="blogName">The name of the blog under the account to which this post will be 
        /// 'reblogged' to.</param>
        /// <param name="caption">Any additional text that is to be added to this post.</param>
        /// <param name="tags">String containing a comma delimited list of tags.</param>
        /// <returns></returns>
        public async Task<bool> ReblogPost(string blogName, RequestParameters parameters) {
            return (await RequestBuilder.Instance.POST(string.Format(APIEndpoints.REBLOG.ToString(), blogName),
                parameters)).StatusCode == HttpStatusCode.Created;
        }
    }
}
