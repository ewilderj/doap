using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Xml;
using System.Xml.Xsl;
using Redland;

namespace Doap {

	public class Viewer
	{
		static Model schemaModel;
	 	static Storage schemaStorage;

		Storage storage;
		Model model;
		Parser parser;
		XmlDocument xml;
		XslTransform style;
		string text;
		
		Redland.Uri DoapSchemaUri = 
			new Redland.Uri ("http://usefulinc.com/ns/doap");

		public Viewer (string uri) 
		{
			parser = new Parser ("raptor");
			loadSchemas ();
			if (uri != null) {
				Load (uri);
			}
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
			if (style == null) {
				style = new XslTransform ();
				style.Load (new XmlTextReader (ass.GetManifestResourceStream ("style.xsl")),
							new XmlUrlResolver (), 
							ass.Evidence);
			}
		}

		public void Load (string uri)
		{
			string txt = null;

			System.Uri u = new System.Uri (uri);
			Util.Debug ("Fetching file from " + uri);
			if (u.IsFile) {
				txt = Util.LoadFile (u.LocalPath);
			} else if (u.Scheme == System.Uri.UriSchemeHttp) {
				txt = Util.FetchUrl (uri);
			} else {
				throw new Exception
					("Unsupported URI scheme. Only file: and http: " +
					 "URIs understood.");
			}

			if (txt == null)
				throw new Exception ("No content found.");

			storage = new Storage ("memory", "data", null);
			model = new Model (storage);
			if (uri == null)
				uri = "file:///tmp/stdin";
			Redland.Uri baseuri = new Redland.Uri (uri);
			
			parser.ParseStringIntoModel (txt, baseuri, model);
		}

		public void Serialize (System.IO.TextWriter writer)
		{
			DoapNs doap = new DoapNs ();
			RdfNs rdf = new RdfNs ();
			FoafNs foaf = new FoafNs ();
			StringWriter sw = new StringWriter ();
			XmlTextWriter w = new XmlTextWriter (sw);

			string[] urltypes = new string[] { "homepage", "mailing-list",
				"download-page", "download-mirror",
				"wiki", "bug-database", "screenshots"
			};
			
			string[] literaltypes = new string[] {
				"created", "os", "programming-language"
			};

			string[] peopletypes = new string [] {
				"maintainer", "developer", "documenter",
				"translator", "tester", "helper"
			};
				
			w.WriteStartDocument ();

			w.WriteStartElement (null, "content", null);

			foreach (Node proj in model.GetSources (rdf ["type"], 
													doap ["Project"])) {

				w.WriteStartElement (null, "project", null);

				Node name = model.GetTarget (proj, doap ["name"]);
				w.WriteStartElement ("name");
				w.WriteString (name.Literal);
				w.WriteEndElement ();

				Node description = model.GetTarget (proj, doap ["description"]);
				w.WriteStartElement ("description");
				w.WriteStartAttribute ("xml", "lang", null);
				w.WriteString (description.Language);
				w.WriteEndAttribute ();
				w.WriteString (description.Literal);
				w.WriteEndElement ();
				
				Node shortdesc = model.GetTarget (proj, doap ["shortdesc"]);
				w.WriteStartElement ("shortdesc");
				w.WriteStartAttribute ("xml", "lang", null);
				w.WriteString (shortdesc.Language);
				w.WriteEndAttribute ();
				w.WriteString (shortdesc.Literal);
				w.WriteEndElement ();

				foreach (string p in urltypes) {
					Node r = model.GetTarget (proj, doap [p]);
					if (r != null) {
						w.WriteStartElement (p);
						w.WriteStartAttribute (null, "href", null);
						w.WriteString (r.Uri.ToString ());
						w.WriteEndElement ();
					}
				}

				foreach (string p in literaltypes) {
					foreach (Node r in model.GetTargets (proj, doap [p])) {
						w.WriteStartElement (p);
						w.WriteString (r.Literal);
						w.WriteEndElement ();
					}
				}

				foreach (string p in peopletypes) {
					foreach (Node r in model.GetTargets (proj, doap [p])) {
						w.WriteStartElement ("person");
						w.WriteStartAttribute (null, "role", null);
						w.WriteString (p);
						w.WriteEndAttribute ();

						Node n = model.GetTarget (r, foaf ["name"]);
						Node hp = model.GetTarget (r, foaf ["homepage"]);

						w.WriteStartAttribute (null, "name", null);
						w.WriteString (n.Literal);
						w.WriteEndAttribute ();

						if (hp != null) {
							w.WriteStartAttribute (null, "homepage", null);
							w.WriteString (hp.Uri.ToString ());
							w.WriteEndAttribute ();
						}
						w.WriteEndElement ();
						w.WriteEndElement ();
					}
				}

				w.WriteEndElement ();
		
			}

			w.WriteEndDocument ();

			// now do XML transform
			System.Xml.XPath.XPathDocument doc = new System.Xml.XPath.XPathDocument (new XmlTextReader (new StringReader (sw.ToString ())));
			style.Transform (doc, null, writer, null);
		}
		
	}
}
