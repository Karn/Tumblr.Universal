using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TumblrUniversal.Tumblr;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;

namespace TumblrUniversal.Services.Request {

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
            IBuffer KeyMaterial = CryptographicBuffer.ConvertStringToBinary(TumblrClient.ConsumerSecretKey + "&" + (string.IsNullOrWhiteSpace(tokenSecret) ? Authentication.TokenSecret : ""), BinaryStringEncoding.Utf8);
            MacAlgorithmProvider HmacSha1Provider = MacAlgorithmProvider.OpenAlgorithm("HMAC_SHA1");
            CryptographicKey MacKey = HmacSha1Provider.CreateKey(KeyMaterial);
            IBuffer DataToBeSigned = CryptographicBuffer.ConvertStringToBinary(signatureBaseString, BinaryStringEncoding.Utf8);
            IBuffer SignatureBuffer = CryptographicEngine.Sign(MacKey, DataToBeSigned);
            return CryptographicBuffer.EncodeToBase64String(SignatureBuffer);
        }
    }
}
