using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tumblr.Universal.Core.Entities {

    public class ActivityItem {
        public int timestamp { get; set; }
        public string date { get; set; }
        public string type { get; set; }
        public string from_tumblelog_name { get; set; }
        public string from_tumblelog_avatar {
            get {
                return "http://api.tumblr.com/v2/blog/" + from_tumblelog_name + ".tumblr.com/avatar/96";
            }
        }
        public int before { get; set; }
        public int from_tumblelog_id { get; set; }
        public string target_post_id { get; set; }
        public string target_post_summary { get; set; }
        public string target_post_type { get; set; }
        public string target_tumblelog_name { get; set; }
        public bool private_channel { get; set; }
        public string media_url { get; set; }
        public string post_type { get; set; }
        public bool followed { get; set; }
        public object post_id { get; set; }
    }
}
