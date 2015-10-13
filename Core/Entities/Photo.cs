using System.Collections.Generic;
using Windows.UI.Xaml;

namespace Tumblr.Universal.Core.Entities {
	public class Photo {
		public string caption { get; set; }
		public List<AltSize> alt_sizes { get; set; }
		public int ColSpan { get; set; }
		public int RowSpan { get; set; }
		public OriginalSize original_size { get; set; }

		public class AltSize {

			public int width { get; set; }
			private int _height = 0;
			public int height {
				get {
					return _height;
				}
				set {
					_height = (value / width) * (int)Window.Current.Bounds.Width;
				}
			}
			public string url { get; set; }
		}

		public class OriginalSize {
			public int width { get; set; }
			public int height { get; set; }
			public string url { get; set; }
		}
	}
}
