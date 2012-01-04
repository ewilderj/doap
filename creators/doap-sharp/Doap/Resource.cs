using System;
using System.Net;

namespace Doap {

	public struct Resource {
		public Uri Uri;

		public Resource(string uri) {
			if (uri != null && uri.Length > 0) {
				Uri = new Uri(uri);
			} else {
				Uri = null;
			}
		}

		public Resource(Uri uri) {
			Uri = uri;
		}

		public override string ToString() {
			return Uri.ToString();
		}
	}
}

