using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Runtime.InteropServices;
using System.Xml;

using Redland;

namespace Doap {


	class CommandLineValidator
	{
		public static bool XmlOutput = true;

		public static void Main (string[] args)
		{
			ArrayList problems;

			Validator v = new Validator ();
			Util.Verbosity = Util.DebugLevel.All;
			Util.Inform ("Loaded schema.");
			if (args.Length == 0) {
				Util.Inform ("Reading from standard input.");
				problems = v.ValidateString ("");
			} else {
				problems = v.Validate (args [0]);
			}

			if (XmlOutput) {
				XmlTextWriter w = new XmlTextWriter (Console.Out);
				w.WriteStartDocument ();
				w.WriteStartElement (null, "Problems", null);
				foreach (Problem p in problems)
				{
					w.WriteWhitespace ("\n");
					w.WriteStartElement (null, "Problem", null);

					w.WriteWhitespace ("\n");
					w.WriteStartElement (null, "Test", null);
					w.WriteString (p.Test);
					w.WriteEndElement ();

					w.WriteWhitespace ("\n");
					w.WriteStartElement (null, "Title", null);
					w.WriteString (p.Title);
					w.WriteEndElement ();

					w.WriteWhitespace ("\n");
					w.WriteStartElement (null, "Description", null);
					w.WriteString (p.Description);
					w.WriteEndElement ();

					w.WriteWhitespace ("\n");
					w.WriteStartElement (null, "Detail", null);
					w.WriteString (p.Detail);
					w.WriteEndElement ();

					w.WriteWhitespace ("\n");
					w.WriteEndElement ();
					w.WriteWhitespace ("\n");
				}
				w.WriteEndElement ();
				w.WriteWhitespace ("\n");
				w.WriteEndDocument ();

			} else {
				foreach (Problem p in problems)
				{
					Console.WriteLine (p.Title + " -- " + p.Description);
					Console.WriteLine (p.Detail);
				}
			}
		}
	}

}
