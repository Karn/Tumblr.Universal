using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tumblr.Universal.Core.Entities {
    internal class ResponseModel {
        
        public class GetInfo {
            public Meta meta { get; set; }
            public UserResponse response { get; set; }
        }

        public class GetActivity {
            public Meta meta { get; set; }
            public ActivityResponse response { get; set; }
        }

        public class Meta {
            public int status { get; set; }
            public string msg { get; set; }
        }

        public class UserResponse {
            public Account user { get; set; }
        }

        public class ActivityResponse {
            public List<ActivityItem> notifications { get; set; }
        }

    }
}
