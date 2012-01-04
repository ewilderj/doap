using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Net;

using Redland;

namespace Doap {

	class Util
	{
		public enum DebugLevel { None, Errors, Debug, All };
		public static DebugLevel Verbosity {
			get { return debug; }
			set { debug = value; }
		}

		private static DebugLevel debug = DebugLevel.Errors;

		public static void Inform (String info)
		{
			Inform (DebugLevel.All, info);
		}

		public static void Error (String info)
		{
			Inform (DebugLevel.Errors, info);
		}

		public static void Debug (String info)
		{
			Inform (DebugLevel.Debug, info);
		}

		public static void Inform (DebugLevel level, String info)
		{
			if (level <= debug) {
				Console.Error.WriteLine (info);
			}
		}

		public static string FetchUrl (string url) 
		{
			HttpWebRequest req = (HttpWebRequest) WebRequest.Create (url);
			req.UserAgent = "DOAP Validator";
			WebResponse resp = req.GetResponse ();
			StreamReader input = new StreamReader (
					resp.GetResponseStream ());
			return input.ReadToEnd ();
		}

		public static string LoadFile (string fname)
		{
			StreamReader input = new StreamReader (
					File.Open (fname, FileMode.Open));
			return input.ReadToEnd ();
		}
	}

}
