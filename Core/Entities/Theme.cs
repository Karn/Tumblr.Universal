using Newtonsoft.Json;

namespace Tumblr.Universal.Core.Entities {

    /// <summary>
    /// Stores JSON deserialization data for the Theme object.
    /// </summary>
    public class Theme {

        /// <summary>
        /// URI to the header image as set by user.
        /// </summary>
        [JsonProperty("header_image_focused")]
        public string HeaderImage { get; set; }

        /// <summary>
        /// URI to the original header image.
        /// </summary>
        [JsonProperty("header_image_scaled")]
        public string ScaledHeaderImage { get; set; }
    }
}
