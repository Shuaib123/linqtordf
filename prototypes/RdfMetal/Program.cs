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
            var properties = new ClassQuerySink(opts.ignoreBnodes, opts.@namespace, new[]{"u"});
            source.RunSparqlQuery(sqGetClasses, properties);
            return properties.bindings.Map(nvc=>nvc["u"]);
        }

        private static IEnumerable<string> GetClassPropertyUris(Opts opts, string classUri)
        {
            var source = new SparqlHttpSource(opts.endpoint);
            var properties = new ClassQuerySink(opts.ignoreBnodes, opts.@namespace, new[] { "s" });

            var sparqlQuery = string.Format(sqGetProperties, classUri);
            source.RunSparqlQuery(sparqlQuery, properties);
            return properties.bindings.Map(nvc=>nvc["s"]);
        }

        private static IEnumerable<Tuple<string, string>> GetClassProperties(Opts opts, string classUri)
        {
            var source = new SparqlHttpSource(opts.endpoint);
            var properties = new ClassQuerySink(opts.ignoreBnodes, null, new[] { "p", "r" });

            var sparqlQuery = string.Format(sqGetProperty, classUri);
            source.RunSparqlQuery(sparqlQuery, properties);
            return properties.bindings
                .Map(nvc => new Tuple<string, string>(nvc["p"], nvc["r"]))
                .Where(t => (!(string.IsNullOrEmpty(t.First) || string.IsNullOrEmpty(t.Second))));
        }

        private static void QueryClasses(Opts opts)
        {
            foreach (var uri in GetClassUris(opts).Distinct())
            {
                Debug.WriteLine("Class " + uri);
                foreach (var t3 in GetClassProperties(opts, uri))
                {
                    Debug.WriteLine(string.Format("  Prop: {0} (r:{1}", t3.First, t3.Second));
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
        string Name { get; set; }
        public OntProp Props { get; set; }
    }

    public class OntProp
    {
        string Name { get; set; }
        string Range { get; set; }
    }
}