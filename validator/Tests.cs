using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Xml;
using Redland;

namespace Doap.Tests {

	public enum Severity { Success, Suggestion, Warning, Error };

	public class Description : Attribute
	{
		string txt;
		public Description (string txt)
		{
			this.txt = txt;
		}
		public string Content {
			get {
				return txt;
			}
		}
	}

	public class Detail : Attribute
	{
		string txt;
		public Detail (string txt)
		{
			this.txt = txt;
		}
		public string Content {
			get {
				return txt;
			}
		}
	}

	public struct TestResult {
		public Severity Severity;
		public string Error;
		public TestResult (Severity s, string e) {
			Severity = s; Error = e;
		}
		public TestResult (Severity s) : this (s, null) { }
	}

	public abstract class ValidatorTest
	{
		Validator validator;
		public Validator Validator {
			set {
				validator = value;
			}
			get {
				return validator;
			}
		}

		public virtual TestResult Run ()
		{
			return new TestResult (Severity.Success, null);
		}
	}

	public abstract class XmlValidatorTest : ValidatorTest
	{
	}

	public abstract class RdfValidatorTest : ValidatorTest
	{
	}

}
