using System;

namespace Doap {
	public class Release {
		public Version Version;
	}

	public struct Version {
		public string Name;
		public DateTime Created;
		public string Revision;
	}
}
