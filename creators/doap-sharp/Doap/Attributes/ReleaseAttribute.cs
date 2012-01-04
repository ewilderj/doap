using System;

namespace Doap.Attributes {

	[AttributeUsage(AttributeTargets.Assembly)]
	public class ReleaseAttribute : Attribute {
		private Release release;

		public Release Release {
			get {
				return release;
			}
		}

		public ReleaseAttribute(string name, string created, string revision) {
			release = new Release();
			release.Version.Name = name;
			release.Version.Created = DateTime.Parse(created);
			release.Version.Revision = revision;
		}
	}
}
