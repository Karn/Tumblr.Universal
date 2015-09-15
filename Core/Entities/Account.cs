using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tumblr.Universal.Core.Entities {

    /// <summary>
    /// Handles 'User' JSON deserialzation.
    /// </summary>
    public class Account {

        /// <summary>
        /// Number of posts liked by the account.
        /// </summary>
        [JsonProperty("likes")]
        public int LikesCount { get; set; }

        /// <summary>
        /// Number of other blogs that are being followed by the account.
        /// </summary>
        [JsonProperty("following")]
        public int FollowingCount { get; set; }

        /// <summary>
        /// Sets/Returns 'true' if NSFW content is allowed.
        /// </summary>
        [JsonProperty("safe_search")]
        public bool AllowNSFW { get; set; }

        /// <summary>
        /// Sets/Returns 'true' if notifications are enabled for the account.
        /// </summary>
        [JsonProperty("push_notifications")]
        public bool NotificationsEnabled { get; set; }

        /// <summary>
        /// List of blogs contained within the account.
        /// </summary>
        [JsonProperty("blogs")]
        public List<Blog> AccountBlogs { get; set; }

    }
}
