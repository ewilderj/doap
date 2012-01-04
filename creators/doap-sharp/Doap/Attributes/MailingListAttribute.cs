using System;

namespace Doap.Attributes {

	[AttributeUsage(AttributeTargets.Assembly)]
	public class MailingListAttribute : Attribute {
		private Resource mailingList;

		public Resource MailingList {
			get {
				return mailingList;
			}
		}

		public MailingListAttribute(string mailingList) {
			this.mailingList = new Resource(mailingList);
		}
	}
}
