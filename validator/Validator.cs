using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Xml;
using Commons.Xml.Relaxng;
using Redland;

namespace Doap {

	public class ValidatorError : Exception {
		public ValidatorError (string s) : base (s)
		{
		}
	}

	struct Problem
	{
		public string Title, Description, Detail;
		public Tests.Severity Severity;
		public string Test;

		public Problem (Doap.Tests.TestResult res, string title, string desc,
				string detail, string testname)
		{
			this.Severity = res.Severity;
			this.Title = title;
			this.Description = desc;
			if (res.Error != null) 
				this.Detail = res.Error;
			else 
				this.Detail = detail;
			this.Test = testname;
		}
	}

	public class Validator
	{
		static Model schemaModel;
	 	static Storage schemaStorage;
		static RelaxngPattern rngSchema;

		Storage storage;
		Model model;
		Parser parser;
		XmlDocument xml;
		string text;
		
		Redland.Uri DoapSchemaUri = 
			new Redland.Uri ("http://usefulinc.com/ns/doap");

		public XmlDocument Xml {
			get {
				return xml;
			}
		}

		public Model Rdf {
			get {
				return model;
			}
		}

		public Model SchemaRdf {
			get {
				return schemaModel;
			}
		}

		public RelaxngPattern RelaxSchema {
			get {
				return rngSchema;
			}
		}

		public string Text {
			get {
				return text;
			}
		}
		
		public Validator () 
		{
			parser = new Parser ("raptor");
			loadSchemas ();
		}
		
		private void loadSchemas ()
		{
			Assembly ass = Assembly.GetExecutingAssembly ();			
			if (schemaModel == null) {
				System.IO.Stream s = ass.GetManifestResourceStream ("doap.rdf");
				schemaStorage = new Storage ("memory", "schema", null);
				schemaModel = new Model (schemaStorage);
				Encoding e = Encoding.GetEncoding ("utf-8");
				StreamReader r = new StreamReader (s, e);
				string txt = r.ReadToEnd ();
				parser.ParseStringIntoModel (txt, DoapSchemaUri, schemaModel);
			}
			if (rngSchema == null) {
				System.IO.Stream s = ass.GetManifestResourceStream ("doap.rng");
				rngSchema = RelaxngPattern.Read (new XmlTextReader (s));
			}
		}

		public ArrayList Validate (string uri)
		{
			string txt = null;
			try {
				System.Uri u = new System.Uri (uri);
				Util.Debug ("Fetching file from " + uri);
				if (u.IsFile) {
					txt = Util.LoadFile (u.LocalPath);
				} else if (u.Scheme == System.Uri.UriSchemeHttp) {
					txt = Util.FetchUrl (uri);
				} else {
					throw new ValidatorError 
						("Unsupported URI scheme. Only file: and http: " +
						 "URIs understood.");
				}
			} catch (Exception e) {
				Util.Error ("Fetch of " + uri + " failed.");
				Util.Debug (e.ToString ());
				return null;
			}
			if (txt == null)
				throw new ValidatorError ("No content found.");
			return ValidateString (txt, uri);
		}

		public ArrayList ValidateString (string str)
		{
			return ValidateString (str, null);
		}

		private ArrayList ValidateString (string str, string uri)
		{
			this.text = str;

			ArrayList results = RunXmlChecks (str); 

			// if XML checks went OK, we can continue with the
			// RDF ones
			if (results.Count == 0) {
			
				storage = new Storage ("memory", "data", null);
				model = new Model (storage);
				if (uri == null)
					uri = "file:///tmp/stdin";
				Redland.Uri baseuri = new Redland.Uri (uri);

				try {
					parser.ParseStringIntoModel (str, baseuri, model);
				} catch (RedlandError r) {
					foreach (LogMessage m in r.Messages) {
						results.Add (new Problem (
									new Tests.TestResult (Tests.Severity.Error),
									"RDF error",
									m.ToString (),
									r.Message, "ParseRDF" ));
					}
				} catch (Exception e) {
					// TODO: add a won't-parse-as-RDF-error
					Util.Error ("Exception caught while parsing as RDF, but continuing anyway.");
					Util.Error (e.ToString ());
				}

				results.AddRange (RunRdfChecks ());
			}

			return results;
		}

		private ArrayList RunXmlChecks (string str)
		{
			ArrayList results = new ArrayList ();
			xml = new XmlDocument ();
			try {
				xml.LoadXml (str);
			} catch (Exception e) {
				results.Add (new Problem (
							new Tests.TestResult (Doap.Tests.Severity.Error),
						"Not well-formed XML",
						"DOAP files must follow the rules of XML syntax.",
						e.Message, "ParseXML"));
			}

			if (results.Count == 0) {
				results.AddRange (RunTests ("Doap.Tests.XmlValidatorTest"));
			}
			return results;
		}

		private ArrayList RunTests (string basetype)
		{
			ArrayList results = new ArrayList ();
			Assembly assembly = Assembly.GetExecutingAssembly ();

			Module [] modules = assembly.GetModules (false);

			Type [] tests = modules [0].FindTypes (
					new TypeFilter (IsValidatorTest),
					basetype);

			foreach (Type test in tests)
			{
				ConstructorInfo cinf = test.GetConstructor (
						new Type [] { });
				Tests.ValidatorTest vt = (Tests.ValidatorTest) cinf.Invoke (
						new object[] { });
				vt.Validator = this;
				Util.Inform (String.Format ("Running {0}", test.FullName));
				Doap.Tests.TestResult rc = vt.Run ();
				if (rc.Severity == Doap.Tests.Severity.Success)
				{
					Util.Inform (String.Format ("{0}: Success.", test.FullName));
				} else {
					Util.Inform (String.Format ("{0}: Failed.", test.FullName));
					string desc = ((Tests.Description)
						Attribute.GetCustomAttribute (
							test, typeof (Tests.Description))).Content;
					string detail = ((Tests.Detail)
						Attribute.GetCustomAttribute (
							test, typeof (Tests.Detail))).Content;

					results.Add (new Problem (rc, desc, detail, 
								rc.Error == null ? "" : rc.Error,
								test.FullName));
				}
			}

			return results;
		}

		public static bool IsValidatorTest (Type m, object criteria)
		{
			string testname = (string) criteria;
			if (m.IsClass && m.BaseType.FullName == testname)
				return true;
			else
				return false;
		}
		
		private ArrayList RunRdfChecks ()
		{
			return RunTests ("Doap.Tests.RdfValidatorTest");
		}

	}
}
