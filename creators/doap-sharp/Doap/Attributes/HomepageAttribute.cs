using System;

using Doap;

namespace Doap.Attributes {

	[AttributeUsage(AttributeTargets.Assembly)]
	public class HomepageAttribute : Attribute {

		private Resource homepage;

		public Resource Homepage {
			get {
				return homepage;
			}
		}

		public HomepageAttribute(string homepage) {
			this.homepage = new Resource(homepage);
		}
	}
}
