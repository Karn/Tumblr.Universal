using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Tumblr.Universal.Core.Entities {

    /// <summary>
    /// Object that holds the deserialized JSON object for a Post datatype.
    /// </summary>
	public class PostItem {

        /// <summary>
        /// The name of the blog of which this Post has been blogged to.
        /// </summary>
		[JsonProperty("blog_name")]
        public string Name { get; set; }

        /// <summary>
        /// Avatar of the blog of which this Post has been blogged to.
        /// </summary>
		public string Avatar {
            get { return "http://api.tumblr.com/v2/blog/" + Name + ".tumblr.com/avatar/96"; }
        }

        /// <summary>
        /// ID of the post relative to the tumblr API.
        /// </summary>
        [JsonProperty("id")]
        public string ID { get; set; }

        /// <summary>
        /// Permalink of the post.
        /// </summary>
        [JsonProperty("post_url")]
        public string PostUrl { get; set; }

        public string object_type { get; set; }
        public string type { get; set; }
        public string timestamp { get; set; }
        public string date { get; set; }
        public string format { get; set; }
        public string reblog_key { get; set; }
        public string[] tags { get; set; }
        public string TagsAsString {
            get {
                try {
                    var converted = "";
                    foreach (var tag in tags)
                        converted += string.Format("#{0} ", tag);
                    return converted.TrimEnd(' ');
                } catch { }
                return "";
            }
        }
        public string source_url { get; set; }
        public string source_title { get; set; }
        public bool liked { get; set; }
        public string state { get; set; }
        public Blog blog { get; set; }
        public string short_url { get; set; }
        public bool followed { get; set; }

        /// <summary>
        /// Number of notes on the post.
        /// </summary>
        [JsonProperty("note_count")]
        public int NoteCount { get; set; }

        /// <summary>
        /// Title of the post.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Body of the post.
        /// </summary>
        [JsonProperty("body")]
        public string Body { get; set; }

        /// <summary>
        /// Excerpt of the link embedded into the post.
        /// </summary>
        [JsonProperty("excerpt")]
        public string Excerpt { get; set; }

        /// <summary>
        /// Returns true of the post can be replied to.
        /// </summary>
        [JsonProperty("can_reply")]
        public bool CanReply { get; set; }

        /// <summary>
        /// User entered capton associated with the post.
        /// </summary>
        [JsonProperty("caption")]
        public string Caption { get; set; }

        /// <summary>
        /// List of photos associated with the post.
        /// </summary>
        [JsonProperty("photos")]
        public ObservableCollection<Photo> PostPhotos { get; set; }
        public string link_url { get; set; }
        public string photoset_layout { get; set; }
        public string permalink_url { get; set; }
        public bool html5_capable { get; set; }
        public string thumbnail_url { get; set; }
        public int thumbnail_width { get; set; }
        public int thumbnail_height { get; set; }
        public object player { get; set; }
        public string video_type { get; set; }
        public string video_url { get; set; }
        public string artist { get; set; }
        public string album { get; set; }
        public int? year { get; set; }
        public string track { get; set; }
        public string track_name { get; set; }
        public string album_art { get; set; }
        public string embed { get; set; }
        public int? plays { get; set; }
        public string audio_url { get; set; }
        public bool? is_external { get; set; }
        public string audio_type { get; set; }
        public string url { get; set; }


        /// <summary>
        /// A user-supplied description of the post.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Main text of the post (Quote).
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }

        private string _source { get; set; }

        /// <summary>
        /// Source of the post (Quote).
        /// </summary>
        [JsonProperty("source")]
        public string Source {
            get {
                return (!string.IsNullOrWhiteSpace(_source) ? "- " + _source : "");
            }
            set {
                _source = value;
            }
        }

        /// <summary>
        /// Returns the first image in the list of alternate images. 
        /// It is also the lowest resolution alternate image.
        /// </summary>
        public Photo.AltSize LowResImage {
            get {
                try {
                    return PostPhotos.First().AlternateSizes.First();
                } catch {
                    return new Photo.AltSize();
                }
            }
        }

        /// <summary>
        /// Returns the last image in the list of alternate images. 
        /// It is also the highest resolution alternate image.
        /// </summary>
        public Photo.AltSize HighResImage {
            get {
                try {
                    return PostPhotos.First().AlternateSizes.Last();
                } catch {
                    return new Photo.AltSize();
                }
            }
        }

        /// <summary>
        /// Name of the blog asking the question.
        /// </summary>
        [JsonProperty("asking_name")]
        public string AskingName { get; set; }

        /// <summary>
        /// Avatar of the blog asking the question.
        /// </summary>
        public string AskingAvatar {
            get {
                if (AskingName == "Anonymous")
                    return "https://secure.assets.tumblr.com/images/anonymous_avatar_96.gif";
                return "http://api.tumblr.com/v2/blog/" + AskingName + ".tumblr.com/avatar/96";
            }
        }

        /// <summary>
        /// The question being asked.
        /// </summary>
        [JsonProperty("question")]
        public string Question { get; set; }

        /// <summary>
        /// Answer to the question being asked.
        /// </summary>
        [JsonProperty("answer")]
        public string Answer { get; set; }
        
        /// <summary>
        /// Detailed summary of the notes associated with the post.
        /// </summary>
        [JsonProperty("notes")]
        public List<Note> PostNotes { get; set; }

        public string special_case { get; internal set; }

        /// <summary>
        /// Returns the name of the blog from which the post has been reblogged from.
        /// </summary>
        [JsonProperty("reblogged_from_name")]
        public string RebloggedFromName { get; set; }

        /// <summary>
        /// Returns the conversation in the chat post.
        /// </summary>
        [JsonProperty("dialogue")]
        public List<DialogueObject> Dialogue { get; set; }

        /// <summary>
        /// Represents the 'DialogObject' object which is deserialized from JSON.
        /// Contains a Label of the conversation and the Phrase associated with the Label.
        /// </summary>
        public class DialogueObject {

            /// <summary>
            /// Returns the label of the speaker in the chat.
            /// </summary>
            [JsonProperty("label")]
            public string Label { get; set; }

            /// <summary>
            /// Returns the phrase said by the speaker in the chat.
            /// </summary>
            [JsonProperty("phrase")]
            public string Phrase { get; set; }
        }

        /// <summary>
        /// Represents the 'Note' object which is deserialized from JSON.
        /// Contains relevant details about a given 'Note' on a post. 
        /// </summary>
        public class Note {

            /// <summary>
            /// Returns the timestamp for the note.
            /// Indicates when the note was added to the post.
            /// </summary>
            [JsonProperty("timestamp")]
            public string Timestamp { get; set; }

            /// <summary>
            /// Returns the name of the blog which resulted in the generation of the note.
            /// </summary>
            [JsonProperty("blog_name")]
            public string Name { get; set; }

            /// <summary>
            /// Returns the avatar of the blog which resulted in the generation of the note.
            /// </summary>
            public string Avatar {
                get {
                    return "http://api.tumblr.com/v2/blog/" + Name + ".tumblr.com/avatar/96";
                }
            }

            /// <summary>
            /// Returns any additional text in the event that the post can be replied to.
            /// </summary>
            [JsonProperty("answer_text")]
            public string Reply { get; set; }
            private string _type { get; set; }
            public string type {
                get {
                    return _type;
                }
                set {
                    if (value.Contains("posted")) {
                        _type = "posted this.";
                    } else if (value.Contains("like")) {
                        _type = "likes this.";
                    } else if (value.Contains("reblog")) {
                        _type = "reblogged this.";
                    } else if (value.Contains("answer")) {
                        _type = "answered: " + Reply;
                    }
                }
            }
        }
    }
}
