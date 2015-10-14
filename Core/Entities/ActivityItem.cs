using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tumblr.Universal.Core.Entities {

    public class ActivityItem {

        /// <summary>
        /// Returns the timestamp corresponding to the notification.
        /// </summary>
        [JsonProperty("timestamp")]
        public int Timestamp { get; internal set; }

        /// <summary>
        /// Returns the date of corresponding to the notification without the time.
        /// Eg. January 1, 2015
        /// </summary>
        public DateTime Date { get; internal set; }

        /// <summary>
        /// Returns the type of notification. Eg. Audio, Photo, etc.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; internal set; }

        /// <summary>
        /// Returns the source of notification.
        /// </summary>
        [JsonProperty("from_tumblelog_name")]
        public string SourceBlog { get; internal set; }

        /// <summary>
        /// Returns the avatar of the blog which is the source of notification.
        /// </summary>
        public string SourceBlogAvatar {
            get {
                return "http://api.tumblr.com/v2/blog/" + SourceBlog + ".tumblr.com/avatar/96";
            }
        }

        /// <summary>
        /// Returns the ID of notification.
        /// </summary>
        [JsonProperty("before")]
        public int Before { get; internal set; }

        /// <summary>
        /// Returns the ID of post associated with the notification.
        /// </summary>
        [JsonProperty("target_post_id")]
        public string PostID { get; internal set; }

        /// <summary>
        /// Returns a summary of post associated with the notification.
        /// </summary>
        [JsonProperty("target_post_summary")]
        public string PostSummary { get; internal set; }

        /// <summary>
        /// Returns the type of post associated with the notification. Similar to the notification Type.
        /// </summary>
        [JsonProperty("target_post_type")]
        public string PostType { get; internal set; }

        /// <summary>
        /// Returns the name of the blog for which the notification was generated.
        /// </summary>
        [JsonProperty("target_tumblelog_name")]
        public string TargetBlog { get; internal set; }

        /// <summary>
        /// Returns the uri of the media associated with the post.
        /// </summary>
        [JsonProperty("media_url")]
        public string MediaUrl { get; internal set; }

        /// <summary>
        /// Returns a boolean indicating wheater the source blog is being followed by the blog for which the 
        /// notification was generated.
        /// </summary>
        [JsonProperty("followed")]
        public bool Followed { get; internal set; }
    }
}
