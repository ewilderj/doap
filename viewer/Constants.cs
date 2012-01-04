using System;
using System.Collections;
using Redland;

namespace Doap {
	public class NsMaker {
		Hashtable terms;
		internal string ns;

		public Node Node (string name)
		{
			if (! terms.ContainsKey (name)) {
				terms [name] = MakeProperty (name);
			}
			return (Node) terms [name];
		}

		public  Node MakeProperty (string name)
		{
			return new Node (new Redland.Uri (ns + name));
		}

		public Node this [string name] {
			get {
				return Node (name);
			}
		}

		public NsMaker (string nspace)
		{
			terms = new Hashtable ();
			ns = nspace;
		}
	}

	public class DoapNs : NsMaker {
		public DoapNs () : base ("http://usefulinc.com/ns/doap#") {
		}
	}

	public class RdfNs : NsMaker {
		public RdfNs () : base ("http://www.w3.org/1999/02/22-rdf-syntax-ns#") {
		}
	}

	public class FoafNs : NsMaker {
		public FoafNs () : base ("http://xmlns.com/foaf/0.1/") {
		}
	}
}
