using Newtonsoft.Json;
using System.Collections.Generic;
using Windows.UI.Xaml;

namespace Tumblr.Universal.Core.Entities {

    /// <summary>
    /// Handles 'Photo' object JSON deserialzation.
    /// </summary>
	public class Photo {

        /// <summary>
        /// Returns the caption associated with the photo.
        /// </summary>
        [JsonProperty("caption")]
        public string PhotoCaption { get; set; }

        /// <summary>
        /// Returns the alternate image sizes associated with the photo.
        /// </summary>
        [JsonProperty("alt_sizes")]
        public List<AltSize> AlternateSizes { get; set; }

        /// <summary>
        /// Handles 'Photo.AltSize' object JSON deserialzation.
        /// </summary>
		public class AltSize {

            /// <summary>
            /// Returns the width of this alternate image.
            /// </summary>
            [JsonProperty("width")]
            public int Width { get; set; }

            private int _height = 0;

            /// <summary>
            /// Returns the height of this alternate image.
            /// </summary>
            [JsonProperty("height")]
            public int Height {
                get { return _height; }
                set {
                    _height = (value / Width) * (int)Window.Current.Bounds.Width;
                }
            }

            /// <summary>
            /// Returns the url of this alternate image.
            /// </summary>
            [JsonProperty("url")]
            public string Url { get; set; }
        }
    }
}
