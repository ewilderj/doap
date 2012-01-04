using System;

namespace Doap.Attributes {

	[AttributeUsage(AttributeTargets.Assembly)]
	public class BugDatabaseAttribute : Attribute {
		private Resource bugDatabase;

		public Resource BugDatabase {
			get {
				return bugDatabase;
			}
		}

		public BugDatabaseAttribute(string bugDatabase) {
			this.bugDatabase = new Resource(bugDatabase);
		}
	}
}
