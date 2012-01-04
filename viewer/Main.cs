using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Runtime.InteropServices;
using System.Xml;

using Redland;

namespace Doap {

	class CommandLineViewer
	{
		public static bool XmlOutput = true;

		public static void Main (string[] args)
		{
			Viewer v;
			Util.Verbosity = Util.DebugLevel.All;
			v = new Viewer (args [0]);
			v.Serialize (Console.Out);
		}
	}

}
