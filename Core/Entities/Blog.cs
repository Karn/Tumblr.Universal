using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tumblr.Universal.Core.Entities {

    /// <summary>
    /// Handles 'Blog' JSON deserialzation and DataBase object manipulations.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Blog {

        /// <summary>
        /// Returns 'true' if this object is the primary blog.
        /// </summary>
        [JsonProperty("primary")]
        public bool IsPrimary { get; set; }

        /// <summary>
        /// Friendly name of the blog.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// The name of this blog.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Number of posts created or reblogged by user.
        /// </summary>
        [JsonProperty("posts")]
        public string PostCount { get; set; }

        /// <summary>
        /// Url to the blog.
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// Description of the blog.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Sets/Returns 'true' if the blog contains adult content.
        /// </summary>
        [JsonProperty("nsfw")]
        public bool IsNsfw { get; set; }

        /// <summary>
        /// Sets/Returns 'true' if the blog allows other users to post questions to their mailbox.
        /// </summary>
        [JsonProperty("ask")]
        public bool AsksEnabled { get; set; }

        /// <summary>
        /// Sets/Returns 'true' if the blog allows other user to post questions to their mailbox anonymously.
        /// </summary>
        [JsonProperty("ask_anon")]
        public bool AllowAnonAsks { get; set; }

        /// <summary>
        /// Sets/Returns 'true' if the blog is being followed by this account.
        /// </summary>
        [JsonProperty("followed")]
        public bool Followed { get; set; }

        /// <summary>
        /// Sets/Returns 'true' if the blog allows for the accounts likes to be visible publically.
        /// </summary>
        [JsonProperty("share_likes")]
        public bool LikesVisible { get; set; }

        /// <summary>
        /// ThemeObject which represents the theming values
        /// </summary>
        [JsonProperty("theme")]
        public Theme BlogTheme { get; set; }

        /// <summary>
        /// Number of other blogs that are following this blog.
        /// </summary>
        [JsonProperty("followers")]
        public int Followers { get; set; }

        /// <summary>
        /// Sets/Returns 'true' if the account is following this blog.
        /// </summary>
        [JsonProperty("following")]
        public bool Following { get; set; }

        /// <summary>
        /// Number of posts that are queued by the blog.
        /// </summary>
        [JsonProperty("queue")]
        public int Queue { get; set; }

        /// <summary>
        /// Number of drafts that are pending by the blog.
        /// </summary>
        [JsonProperty("drafts")]
        public int Drafts { get; set; }

        /// <summary>
        /// Email/Tumblr Username associated with the account.
        /// </summary>
        public string AccountEmail { get; set; }

        /// <summary>
        /// Reference to the header image of the blog.
        /// </summary>
        public string HeaderImage { get; set; }

        /// <summary>
        /// The uri to the blogs avatar.
        /// </summary>
        public string Avatar { get { return  (AvatarItems == null || AvatarItems.Count == 0) ? "http://api.tumblr.com/v2/blog/" + Name + ".tumblr.com/avatar/128" : AvatarItems.First().URL; } }

        /// <summary>
        /// Contains a list of avatars with various dimensions.
        /// </summary>
        [JsonProperty("avatar")]
        public List<AvatarItem> AvatarItems { get; set; }

        /// <summary>
        /// Class that represents an avatar of a given width and height.
        /// </summary>
        public class AvatarItem {

            /// <summary>
            /// The url to the avatar with this size.
            /// </summary>
            [JsonProperty("url")]
            public string URL { get; set; }
        }

    }
}
