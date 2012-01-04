using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;

using Doap;
using Doap.Attributes;

namespace DoapSharp {
	  public class DoapWriter {

		private ProjectProperties properties;
		private Hashtable attributes = new Hashtable();

		public static void Main(string [] args) {
			try {
				string assemblyName = args[0];
				string outfile = args[1];

				Assembly assembly = Assembly.LoadFrom(assemblyName);
				using (Stream stream = File.Create(outfile)) {
					DoapWriter writer = new DoapWriter(assembly);
					writer.WriteTo(stream);
				}
			} catch (Exception e) {
				Console.WriteLine(e);
			}
		}

		public DoapWriter(Assembly assembly) {
			foreach (object attribute in assembly.GetCustomAttributes(true)) {
				attributes[attribute.GetType()] = attribute;
			}
			properties = new ProjectProperties(attributes);
		}

		private void WriteTo(Stream stream) {
			properties.WriteTo(stream);
		}
	}
}

