using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Tumblr.Universal.Core;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Tumblr.Universal.Services.Request {

    /// <summary>
    /// Creates and signs an authenticated request to the Tumblr API.
    /// </summary>
    internal class RequestBuilder {

        private static RequestBuilder _requestBuilder;

        /// <summary>
        /// Returns an instace of the TumblrClient object.
        /// </summary>
        public static RequestBuilder Instance {
            get {
                if (_requestBuilder == null)
                    _requestBuilder = new RequestBuilder();
                return _requestBuilder;
            }
        }

        /// <summary>
        /// Primary constructor.
        /// </summary>
        private RequestBuilder() {

        }


        /// <summary>
        /// Generates an unique token to use with an request.
        /// </summary>
        /// <returns>A randomly generated integer value.</returns>
        public string GetNonce() {
            return (new Random()).Next(123400, 9999999).ToString();
        }

        /// <summary>
        /// Returns the current time as a timestamp for use with a request.
        /// </summary>
        /// <returns>A integer that represents the timestamp in seconds.</returns>
        public string GetTimeStamp() {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        /// <summary>
        /// Encode a given string for use awith javascript queries and post bodies.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string Encode(string str) {
            str = WebUtility.UrlEncode(str);

            str = str.Replace("'", "%27").Replace("(", "%28").Replace(")", "%29").Replace("*", "%2A").Replace("!", "%21").Replace("%7e", "~").Replace("+", "%20");

            StringBuilder sbuilder = new StringBuilder(str);
            for (int i = 0; i < sbuilder.Length; i++) {
                if (sbuilder[i] == '%') {
                    if (Char.IsLetter(sbuilder[i + 1]) || Char.IsLetter(sbuilder[i + 2])) {
                        sbuilder[i + 1] = Char.ToUpper(sbuilder[i + 1]);
                        sbuilder[i + 2] = Char.ToUpper(sbuilder[i + 2]);
                    }
                }
            }

            return sbuilder.ToString();
        }

        /// <summary>
        /// Generates a HMAC_SHA1 signature to authenticate API requests with the ConsumerSecretKey and if possible the session token associated with the account.
        /// </summary>
        /// <param name="signatureBaseString">The string that is being used to generate the signature.</param>
        /// <param name="tokenSecret">The session token associated with the account.</param>
        /// <returns></returns>
        public string GenerateSignature(string signatureBaseString, string tokenSecret = "") {
            IBuffer KeyMaterial = CryptographicBuffer.ConvertStringToBinary(TumblrClient.ConsumerSecretKey + "&" + (string.IsNullOrWhiteSpace(tokenSecret) ? TumblrClient.AccessSecretToken : ""), BinaryStringEncoding.Utf8);
            MacAlgorithmProvider HmacSha1Provider = MacAlgorithmProvider.OpenAlgorithm("HMAC_SHA1");
            CryptographicKey MacKey = HmacSha1Provider.CreateKey(KeyMaterial);
            IBuffer DataToBeSigned = CryptographicBuffer.ConvertStringToBinary(signatureBaseString, BinaryStringEncoding.Utf8);
            IBuffer SignatureBuffer = CryptographicEngine.Sign(MacKey, DataToBeSigned);
            return CryptographicBuffer.EncodeToBase64String(SignatureBuffer);
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

        public async Task<HttpResponseMessage> GET(string URL, RequestParameters parameters) {
            try {
                using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate })) {

                    string nonce = GetNonce();
                    string timeStamp = GetTimeStamp();
                    var requestParameters = parameters.ToString();

                    parameters.AppendDefault(nonce, timeStamp);
                    string signatureParameters = "GET&" + Encode(URL) + "&" +
                        Encode(parameters.ToString());

                    var authenticationData = parameters.GenerateAuthenticationData(GenerateSignature(signatureParameters));

                    var requestMessage = new HttpRequestMessage() {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(URL +
                        (!string.IsNullOrEmpty(requestParameters) ? "?" + requestParameters : ""))
                    };

                    requestMessage.Headers.Add("Accept-Encoding", "gzip,deflate");
                    requestMessage.Headers.Add("User-Agent", "Android");
                    requestMessage.Headers.Add("X-Version", "device/3.8.8.41/0/4.4.4");
                    requestMessage.Headers.Add("X-YUser-Agent", "Mozilla/5.0 (Linux; Android 4.4; Nexus 5 Build/KRT16M)/Tumblr/device/3.8.8.41/0/4.4.4");
                    requestMessage.Headers.Authorization = new AuthenticationHeaderValue("OAuth", authenticationData);
                    requestMessage.Headers.IfModifiedSince = DateTime.UtcNow.Date;

                    return await client.SendAsync(requestMessage);
                }
            } catch (Exception ex) {
                return new HttpResponseMessage() { StatusCode = HttpStatusCode.NotFound };
            }
        }

        public async Task<HttpResponseMessage> POST(string URL, RequestParameters parameters) {
            try {
                using (var client = new HttpClient() { MaxResponseContentBufferSize = int.MaxValue }) {
                    client.DefaultRequestHeaders.ExpectContinue = false;

                    string nonce = GetNonce();
                    string timeStamp = GetTimeStamp();

                    var requestMessage = new HttpRequestMessage() {
                        Method = HttpMethod.Post,
                        RequestUri = new Uri(URL)
                    };

                    requestMessage.Content = new StringContent(parameters.ToString());

                    parameters.AppendDefault(nonce, timeStamp);
                    var requestParameters = "POST&" + Encode(URL) + "&" +
                        Encode(parameters.ToString());

                    var authenticationData = parameters.GenerateAuthenticationData(GenerateSignature(requestParameters));

                    requestMessage.Headers.Authorization = new AuthenticationHeaderValue("OAuth", authenticationData);
                    requestMessage.Headers.IfModifiedSince = DateTime.UtcNow;
                    requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                    return await client.SendAsync(requestMessage);
                }
            } catch (Exception ex) {
                return new HttpResponseMessage() { StatusCode = HttpStatusCode.NotFound };
            }
        }

        public async Task<HttpResponseMessage> POST(string URL, StorageFile file, RequestParameters parameters) {
            try {
                using (var client = new HttpClient() { MaxResponseContentBufferSize = int.MaxValue }) {
                    client.DefaultRequestHeaders.ExpectContinue = false;

                    string nonce = GetNonce();
                    string timeStamp = GetTimeStamp();

                    var requestParameters = "POST&" + Encode(URL) + "&" +
                        Encode(parameters.ToString());

                    var authenticationData = parameters.GenerateAuthenticationData(GenerateSignature(requestParameters));

                    var requestMessage = new HttpRequestMessage() {
                        Method = HttpMethod.Post,
                        RequestUri = new Uri(URL)
                    };

                    requestMessage.Headers.Authorization = new AuthenticationHeaderValue("OAuth", authenticationData);
                    string boundary = "---------------------------7dd36f1721dc0";
                    requestMessage.Content = new ByteArrayContent(await BuildByteArray(boundary, file,
                        parameters));
                    requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");
                    requestMessage.Content.Headers.ContentType.Parameters.Add(new NameValueHeaderValue("boundary", boundary));

                    return await client.SendAsync(requestMessage);
                }
            } catch (Exception ex) {
                return new HttpResponseMessage() { StatusCode = HttpStatusCode.NotFound };
            }
        }
        private static async Task<byte[]> BuildByteArray(string boundary, StorageFile file, RequestParameters parameters) {
            var paramsToMFC = string.Join("--" + boundary + "\r\n",
                parameters.Select(p =>
                string.Format("Content-Disposition: form-data; name=\"{0}\"\r\n" +
                "\r\n" +
                "{1}\r\n"
                , p.Key, p.Value))
                );

            byte[] fileBytes = null;

            using (IRandomAccessStreamWithContentType stream = await file.OpenReadAsync()) {
                fileBytes = new byte[stream.Size];
                using (DataReader reader = new DataReader(stream)) {
                    await reader.LoadAsync((uint)stream.Size);
                    reader.ReadBytes(fileBytes);
                }
            }

            byte[] firstBytes = Encoding.UTF8.GetBytes(string.Format(
                "--{1}\r\n" +
                paramsToMFC +
                "--{1}\r\n" +
                "Content-Disposition: form-data; name=\"{2}\"; filename=\"{3}\"\r\n" +
                "Content-Type: {4}\r\n" +
                "\r\n",
                boundary,
                boundary,
                "data[0]",
                file.Name,
                file.ContentType));

            byte[] lastBytes = Encoding.UTF8.GetBytes(String.Format(
                "\r\n" +
                "--{0}--\r\n",
                boundary));

            int contentLength = firstBytes.Length + fileBytes.Length + lastBytes.Length;
            byte[] contentBytes = new byte[contentLength];

            // Join the 3 arrays into 1.
            Array.Copy(firstBytes, 0,
                contentBytes, 0,
                firstBytes.Length);
            Array.Copy(fileBytes, 0,
                contentBytes, firstBytes.Length,
                fileBytes.Length);
            Array.Copy(
                lastBytes, 0,
                contentBytes, firstBytes.Length + fileBytes.Length,
                lastBytes.Length);

            return contentBytes;

        }

        private static byte[] BuildByteArray(string name, string fileName, byte[] fileBytes, string boundary) {
            // Create multipart/form-data headers.
            byte[] firstBytes = Encoding.UTF8.GetBytes(String.Format(
                "--{0}\r\n" +
                "Content-Disposition: form-data; name=\"type\"\r\n" +
                "\r\n" +
                "photo\r\n" +
                "--{1}\r\n" +
                "Content-Disposition: form-data; name=\"{2}\"; filename=\"{3}\"\r\n" +
                "Content-Type: image/jpeg\r\n" +
                "\r\n",
                boundary,
                boundary,
                name,
                fileName));

            byte[] lastBytes = Encoding.UTF8.GetBytes(String.Format(
                "\r\n" +
                "--{0}--\r\n",
                boundary));

            int contentLength = firstBytes.Length + fileBytes.Length + lastBytes.Length;
            byte[] contentBytes = new byte[contentLength];

            // Join the 3 arrays into 1.
            Array.Copy(firstBytes, 0,
                contentBytes, 0,
                firstBytes.Length);
            Array.Copy(fileBytes, 0,
                contentBytes, firstBytes.Length,
                fileBytes.Length);
            Array.Copy(
                lastBytes, 0,
                contentBytes, firstBytes.Length + fileBytes.Length,
                lastBytes.Length);

            return contentBytes;
        }
    }
}
