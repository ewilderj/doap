using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Xml;
using Commons.Xml.Relaxng;
using Redland;

namespace Doap.Tests.Xml {

	using Doap.Tests;

	[Description("Always succeeds")]
	[Detail("This test always succeeds, you can ignore it.")]
	public class AlwaysTrue : XmlValidatorTest
	{

		public override TestResult Run ()
		{
			return new TestResult (Severity.Success);
		}
	}

	[Description("XML syntax validation")]
	[Detail("This test checks to see that the DOAP file validates against a restricted XML syntax, which guarantees its validity.")]
	public class RelaxValidate : XmlValidatorTest
	{
		public override TestResult Run ()
		{
			RelaxngValidatingReader v = new RelaxngValidatingReader (
					new XmlTextReader (new StringReader (Validator.Text)),
					Validator.RelaxSchema);

			try {
				while (v.Read ()) { }
			} catch (RelaxngException e) {
				return new TestResult (Severity.Error, e.Message);
			}

			return new TestResult (Severity.Success);
		}
	}

}
