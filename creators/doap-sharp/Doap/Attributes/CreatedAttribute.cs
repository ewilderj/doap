using System;

namespace Doap.Attributes {

	[AttributeUsage(AttributeTargets.Assembly)]
	public class CreatedAttribute : Attribute {
		private DateTime created;

		public DateTime Created {
			get {
				return created;
			}
		}

		public CreatedAttribute(string created) {
			if (created != null && created.Length > 0) {
				this.created = DateTime.Parse(created);
			}
		}
	}
}
