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

        private static void Main(string[] args)
        {
            Opts opts = ProcessOptions(args);
            QueryClasses(opts);
        }

        private static void QueryClasses(Opts opts)
        {
            MetadataRetriever mr = new MetadataRetriever(opts);
            foreach (var c in mr.GetClasses())
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

    }
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
}