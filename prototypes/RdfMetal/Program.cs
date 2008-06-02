using System;
using System.Collections.Generic;
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
PREFIX foaf: <http://xmlns.com/foaf/0.1/>

SELECT DISTINCT ?s
WHERE
{
?s rdfs:domain <{0}>.
}";
        static string sqDescribeClass = @"DESCRIBE {0}";
        #endregion

        private static void Main(string[] args)
        {
            Opts opts = ProcessOptions(args);
            QueryClasses(opts);
            //QueryProperties(opts);
        }

        private static IEnumerable<string> GetClassUris(Opts opts)
        {
            var source = new SparqlHttpSource(opts.endpoint);
            var properties = new ClassQuerySink(opts.ignoreBnodes, opts.@namespace, "u");
            source.RunSparqlQuery(sqGetClasses, properties);
            return properties.ClassUris.Map(u => u.AbsoluteUri);
        }

        private static IEnumerable<string> GetClassPropertyUris(Opts opts, string classUri)
        {
            var source = new SparqlHttpSource(opts.endpoint);
            var properties = new ClassQuerySink(opts.ignoreBnodes, opts.@namespace, "s");
            #region x

            var x1 =
                @"
PREFIX owl:  <http://www.w3.org/2002/07/owl#>
PREFIX rdfs:  <http://www.w3.org/2000/01/rdf-schema#>
PREFIX foaf: <http://xmlns.com/foaf/0.1/>

SELECT DISTINCT ?s
WHERE
{
?s rdfs:domain <";
            var x2 = @">.
}";
            #endregion
            source.RunSparqlQuery(x1 + classUri + x2 , properties);
            return properties.ClassUris.Map(u => u.AbsoluteUri);
        }

        private static void QueryClasses(Opts opts)
        {
            foreach (var uri in GetClassUris(opts).Distinct())
            {
                Debug.WriteLine("Class " + uri);
                foreach (var propertyUri in GetClassPropertyUris(opts, uri))
                {
                    Debug.WriteLine("  Prop: " + propertyUri);
                }
            }
        }

        public static void QueryProperties(Opts opts)
        {
            var source = new SparqlHttpSource(opts.endpoint);
            var properties = new ClassQuerySink(opts.ignoreBnodes, opts.@namespace, "p");
            source.RunSparqlQuery(sqGetProperties, properties);
            foreach (Uri uri in properties.ClassUris)
            {
                Debug.WriteLine(uri.AbsoluteUri);
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
        private readonly string varName;

        public ClassQuerySink(bool ignoreBnodes, string @namespace, string varName)
        {
            this.ignoreBnodes = ignoreBnodes;
            this.@namespace = @namespace;
            this.varName = varName;
        }

        public List<Uri> ClassUris = new List<Uri>();

        public override bool Add(VariableBindings result)
        {
            Resource resource = result[varName];
            if (!string.IsNullOrEmpty(resource.Uri))
            {
                if (string.IsNullOrEmpty(@namespace) || resource.Uri.StartsWith(@namespace))
                    ClassUris.Add(new Uri(resource.Uri));
            }
            else if (resource is BNode && !ignoreBnodes)
            {
                var bn = resource as BNode;
                if (string.IsNullOrEmpty(@namespace) || bn.LocalName.StartsWith(@namespace))
                    ClassUris.Add(new Uri(bn.LocalName));
            }
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
}