using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using Mono;
using Mono.GetOptions;
using SemWeb;
using SemWeb.Query;
using SemWeb.Remote;

namespace RdfMetal
{

    internal class Program
    {
        #region queries
        static string sqGetClasses =
            @"
PREFIX owl:  <http://www.w3.org/2002/07/owl#>

SELECT ?u
WHERE
	{
	?u a owl:Class .
	}
";
        static string sqGetProperties =
            @"
PREFIX owl:  <http://www.w3.org/2002/07/owl#>
PREFIX rdfs:  <http://www.w3.org/2000/01/rdf-schema#>

SELECT DISTINCT ?s
WHERE
{{
?s rdfs:domain <{0}>.
}}";
        static string sqGetProperty =
            @"
PREFIX owl:  <http://www.w3.org/2002/07/owl#>
PREFIX rdfs:  <http://www.w3.org/2000/01/rdf-schema#>

SELECT DISTINCT ?p ?r
WHERE
{{
?p a owl:DatatypeProperty .
?p rdfs:domain <{0}>.
?p rdfs:range ?r.
}}";
        static string sqGetObjectProperty =
            @"
PREFIX owl:  <http://www.w3.org/2002/07/owl#>
PREFIX rdfs:  <http://www.w3.org/2000/01/rdf-schema#>

SELECT DISTINCT ?p ?r
WHERE
{{
?p a owl:ObjectProperty .
?p rdfs:domain <{0}>.
?p rdfs:range ?r.
}}";
        static string sqDescribeClass = @"DESCRIBE {0}";
        #endregion

        private static void Main(string[] args)
        {
            Opts opts = ProcessOptions(args);
            QueryClasses(opts);
        }

        private static IEnumerable<string> GetClassUris(Opts opts)
        {
            var source = new SparqlHttpSource(opts.endpoint);
            var properties = new ClassQuerySink(opts.ignoreBnodes, opts.@namespace, new[] { "u" });
            source.RunSparqlQuery(sqGetClasses, properties);
            return properties.bindings.Map(nvc => nvc["u"]);
        }

        private static IEnumerable<string> GetClassPropertyUris(Opts opts, string classUri)
        {
            var source = new SparqlHttpSource(opts.endpoint);
            var properties = new ClassQuerySink(opts.ignoreBnodes, opts.@namespace, new[] { "s" });

            var sparqlQuery = string.Format(sqGetProperties, classUri);
            source.RunSparqlQuery(sparqlQuery, properties);
            return properties.bindings.Map(nvc => nvc["s"]);
        }

        private static OntClass GetClass(Opts opts, string classUri)
        {
            Uri u = new Uri(classUri);
            var source = new SparqlHttpSource(opts.endpoint);
            var properties = new ClassQuerySink(opts.ignoreBnodes, null, new[] { "p", "r" });

            var sparqlQuery = string.Format(sqGetProperty, classUri);
            source.RunSparqlQuery(sparqlQuery, properties);
            var q1 = properties.bindings
                .Map(nvc => new Tuple<string, string>(nvc["p"], nvc["r"]))
                .Where(t => (!(string.IsNullOrEmpty(t.First) || string.IsNullOrEmpty(t.Second))));

            sparqlQuery = string.Format(sqGetObjectProperty, classUri);
            source.RunSparqlQuery(sparqlQuery, properties);
            var q2 = properties.bindings
                .Map(nvc => new Tuple<string, string>(nvc["p"], nvc["r"]))
                .Where(t => (!(string.IsNullOrEmpty(t.First) || string.IsNullOrEmpty(t.Second))));
            var ops = q2.Map(t => new OntProp
                                      {
                                          Uri = t.First.Trim(),
                                          IsObjectProp = true,
                                          Name = GetNameFromUri(t.First),
                                          Range = GetNameFromUri(t.Second)
                                      });
            var dps = q1.Map(t => new OntProp
                                      {
                                          Uri = t.First.Trim(),
                                          IsObjectProp = false,
                                          Name = GetNameFromUri(t.First),
                                          Range = GetNameFromUri(t.Second)
                                      });
            var comparer = new PropComparer();
            var d = new Dictionary<string, OntProp>();
            var props = ops.Union(dps);
            foreach (OntProp prop in props)
            {
                d[prop.Uri] = prop;
            }
            OntClass result = new OntClass
                                  {
                                      Name = u.Segments[u.Segments.Length - 1],
                                      Uri = classUri,
                                      Props = d.Values.Where(p => NamespaceMatches(opts, p)).ToArray()
                                  };
            return result;
        }

        private static bool NamespaceMatches(Opts opts, OntProp p)
        {
            if (string.IsNullOrEmpty(opts.@namespace))
            {
                return true;
            }
            return p.Uri.StartsWith(opts.@namespace);
        }

        private static string GetNameFromUri(string s)
        {
            Uri u = new Uri(s);
            if (!string.IsNullOrEmpty(u.Fragment))
            {
                return u.Fragment.Substring(1);
            }
            return u.Segments[u.Segments.Length - 1];
        }

        private static IEnumerable<OntClass> GetClasses(Opts opts)
        {
            return GetClassUris(opts)
                .Distinct()
                .Where(s => !string.IsNullOrEmpty(s))
                .Map(u => GetClass(opts, u));
        }
        private static void QueryClasses(Opts opts)
        {
            foreach (var c in GetClasses(opts))
            {
                Debug.WriteLine("Class " + c.Name);
                foreach (OntProp prop in c.Props)
                {
                    Debug.WriteLine(string.Format("\t{0} ({1})", prop.Name, prop.Range));
                }
            }
        }

        private static Opts ProcessOptions(string[] args)
        {
            Opts opts = new Opts();
            opts.ProcessArgs(args);
            return opts;
        }

        #region Nested type: Opts

        public class Opts : Options
        {
            [Option("The SPARQL endpoint to query.", 'e', "endpoint")]
            public string endpoint = "http://DBpedia.org/sparql";

            [Option("An XML Namespace to extract classes from.", 'n', "namespace")]
            public string @namespace = "";

            [Option("Ignore BNodes.", 'i', "ignorebnodes")]
            public bool ignoreBnodes = false;

            public Opts()
            {
                base.ParsingMode = OptionsParsingMode.Both;
            }

        }

        #endregion
    }

    public class ClassQuerySink : QueryResultSink
    {
        private readonly bool ignoreBnodes;
        private readonly string @namespace;
        private readonly string[] varNames;

        public ClassQuerySink(bool ignoreBnodes, string @namespace, string[] varNames)
        {
            this.ignoreBnodes = ignoreBnodes;
            this.@namespace = @namespace;
            this.varNames = varNames;
        }

        public List<NameValueCollection> bindings = new List<NameValueCollection>();

        public override bool Add(VariableBindings result)
        {
            NameValueCollection nvc = new NameValueCollection();

            foreach (string varName in varNames)
            {
                Resource resource = result[varName];
                if (!string.IsNullOrEmpty(resource.Uri))
                {
                    if (string.IsNullOrEmpty(@namespace) || resource.Uri.StartsWith(@namespace))
                    {
                        nvc[varName] = resource.Uri;
                    }
                }
                else if (resource is BNode && !ignoreBnodes)
                {
                    var bn = resource as BNode;
                    if (string.IsNullOrEmpty(@namespace) || bn.LocalName.StartsWith(@namespace))
                    {
                        nvc[varName] = bn.LocalName;
                    }
                }

            }
            bindings.Add(nvc);
            return true;
        }
    }
    public class ClassDetailQuerySink : StatementSink
    {
        public ClassDetailQuerySink()
        {
        }

        public List<Uri> ls = new List<Uri>();

        public bool Add(Statement s)
        {
            string output = string.Format("{0} {1} {2}", s.Subject, s.Predicate, s.Object);
            Debug.WriteLine(output);
            return true;
        }
    }

    public enum OwlElementType
    {
        OwlClass,
        OwlDatatypeProperty,
        OwlObjectProperty
    }

    public static class FunctionalExtensions
    {
        public static IEnumerable<R> Map<T, R>(this IEnumerable<T> seq, Func<T, R> f)
        {
            foreach (T a in seq)
            {
                yield return f(a);
            }
        }
    }

    #region tuples

    public class Tuple<T>
    {
        public Tuple(T first)
        {
            First = first;
        }

        public T First { get; set; }
    }

    public class Tuple<T, T2> : Tuple<T>
    {
        public Tuple(T first, T2 second)
            : base(first)
        {
            Second = second;
        }

        public T2 Second { get; set; }
    }

    public class Tuple<T, T2, T3> : Tuple<T, T2>
    {
        public Tuple(T first, T2 second, T3 third)
            : base(first, second)
        {
            Third = third;
        }

        public T3 Third { get; set; }
    }

    public class Tuple<T, T2, T3, T4> : Tuple<T, T2, T3>
    {
        public Tuple(T first, T2 second, T3 third, T4 fourth)
            : base(first, second, third)
        {
            Fourth = fourth;
        }

        public T4 Fourth { get; set; }
    }

    #endregion

    public class OntClass
    {
        public string Uri { get; set; }
        public string Name { get; set; }
        public OntProp[] Props { get; set; }
    }

    public class PropComparer : IEqualityComparer<OntProp>
    {
        public bool Equals(OntProp x, OntProp y)
        {
            var b = x.Uri.Equals(y.Uri);
            return b;
        }

        public int GetHashCode(OntProp obj)
        {
            return obj.GetHashCode();
        }
    }

    public class OntProp
    {
        public override bool Equals(object obj)
        {
            OntProp x = obj as OntProp;
            if (x == null)
                return false;
            return this.Uri.Equals(x.Uri);
        }

        public string Uri { get; set; }
        public bool IsObjectProp { get; set; }
        public string Name { get; set; }
        public string Range { get; set; }
    }
}