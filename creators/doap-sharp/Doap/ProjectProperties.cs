using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Xml;

using Doap.Attributes;

namespace Doap {

	public struct Namespace {
		public const string DOAP  = "http://usefulinc.com/ns/doap#";
	 	public const string RDF   = "http://www.w3.org/1999/02/22-rdf-syntax-ns#";
		public const string FOAF  = "http://xmlns.com/foaf/0.1/";
		public const string XMLNS = "http://www.w3.org/2000/xmlns/";
	}

	public class ProjectProperties {

		private static readonly CultureInfo defaultCulture = CultureInfo.CreateSpecificCulture("en-US");

		public string Name;
		public Resource Homepage;
		public DateTime Created;
		public LocalizedString ShortDesc;
		public LocalizedString Description;
		public Resource MailingList;
		public Resource BugDatabase;
		public Release Release;

		public ProjectProperties(IDictionary attributes) {

			// standard attributes
			AssemblyDescriptionAttribute assemblyDescriptionAttribute = 
				attributes[typeof(AssemblyDescriptionAttribute)] as AssemblyDescriptionAttribute;
			if (assemblyDescriptionAttribute != null) {
				Description = new LocalizedString(defaultCulture, assemblyDescriptionAttribute.Description);
			}

			AssemblyTitleAttribute assemblyTitleAttribute = 
				attributes[typeof(AssemblyTitleAttribute)] as AssemblyTitleAttribute;
			if (assemblyTitleAttribute != null) {
				Name = assemblyTitleAttribute.Title;
			}

			// DOAP attributes
			BugDatabaseAttribute bugDatabaseAttribute = 
				attributes[typeof(BugDatabaseAttribute)] as BugDatabaseAttribute;
			if (bugDatabaseAttribute != null) {
				BugDatabase = bugDatabaseAttribute.BugDatabase;
			}

			CreatedAttribute createdAttribute = 
				attributes[typeof(CreatedAttribute)] as CreatedAttribute;
			if (createdAttribute != null) {
				Created = createdAttribute.Created;
			}

			HomepageAttribute homepageAttribute = 
				attributes[typeof(HomepageAttribute)] as HomepageAttribute;
			if (homepageAttribute != null) {
				Homepage = homepageAttribute.Homepage;
			}

			MailingListAttribute mailingListAttribute = 
				attributes[typeof(MailingListAttribute)] as MailingListAttribute;
			if (mailingListAttribute != null) {
				MailingList = mailingListAttribute.MailingList;
			}

			ReleaseAttribute releaseAttribute = 
				attributes[typeof(ReleaseAttribute)] as ReleaseAttribute;
			if (releaseAttribute != null) {
				Release = releaseAttribute.Release;
			}

			LicenseAttribute licenseAttribute = 
				attributes[typeof(LicenseAttribute)] as LicenseAttribute;
			MaintainerAttribute MaintainerAttribute = 
				attributes[typeof(MaintainerAttribute)] as MaintainerAttribute;
			ScreenshotsAttribute screenshotsAttribute = 
				attributes[typeof(ScreenshotsAttribute)] as ScreenshotsAttribute;
			ShortDescriptionAttribute shortDescriptionAttribute = 
				attributes[typeof(ShortDescriptionAttribute)] as ShortDescriptionAttribute;
		}

		public void WriteTo(Stream stream) {

			XmlTextWriter writer = new XmlTextWriter(new StreamWriter(stream));
			writer.Formatting = Formatting.Indented;
			writer.WriteStartDocument(true);
			writer.WriteStartElement("Project", Namespace.DOAP);
			writer.WriteAttributeString("xmlns", "rdf", Namespace.XMLNS, Namespace.RDF);
			writer.WriteAttributeString("xmlns", "foaf", Namespace.XMLNS, Namespace.FOAF);

			writer.WriteElementString("name", Name);

			writer.WriteStartElement("homepage");
			writer.WriteAttributeString("rdf", "resource", Namespace.RDF, Homepage.ToString());
			writer.WriteEndElement(); // homepage

			writer.WriteElementString("created", DateTimeToString(Created));

			writer.WriteStartElement("mailing-list");
			writer.WriteAttributeString("rdf", "resource", Namespace.RDF, MailingList.ToString());
			writer.WriteEndElement(); // mailing-list

			if (Description != null && Description.Length > 0) {
				writer.WriteStartElement("description");
				writer.WriteAttributeString("xml:lang", Description.Culture.Name);;
				writer.WriteString(Description.Text);
				writer.WriteEndElement(); // description
			}

			if (Release != null) {
				writer.WriteStartElement("release");
				writer.WriteStartElement("Version");
				writer.WriteElementString("name", Release.Version.Name);
				writer.WriteElementString("created", DateTimeToString(Release.Version.Created));
				writer.WriteElementString("revision", Release.Version.Revision);
				writer.WriteEndElement(); // Version
				writer.WriteEndElement(); // release
			}
				
			writer.WriteEndElement();
			writer.WriteEndDocument();
			writer.Close();
		}

		private static string DateTimeToString(DateTime dateTime) {
			return string.Format("{0:D4}-{1:D2}-{2:D2}",
				dateTime.Year, dateTime.Month, dateTime.Day);
		}
	}

	public class LocalizedString {

		public string Text;
		public CultureInfo Culture;

		public LocalizedString(CultureInfo culture, string text) {
			Text = text;
			Culture = culture;
		}

		public int Length {
			get {
				return Text.Length;
			}
		}
	}
}
