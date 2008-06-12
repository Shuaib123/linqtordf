using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using SemWeb;
using SemWeb.Query;
/*
 * -i -n http://xmlns.com/foaf/0.1/ -o ..\..\out.cs -m meta.xml
 * -e:http://DBpedia.org/sparql -i -n http://xmlns.com/foaf/0.1/ -o ..\..\out.cs -m meta.xml
 * */
namespace RdfMetal
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Options opts = ProcessOptions(args);
            QueryClasses(opts);
        }

        private static void QueryClasses(Options opts)
        {
            IEnumerable<OntologyClass> classes = null;

            if (!string.IsNullOrEmpty(opts.endpoint))
            {
                var mr = new MetadataRetriever(opts);
                classes = new List<OntologyClass>(mr.GetClasses());
            }

            if (!string.IsNullOrEmpty(opts.metadata) && classes != null)
            {
                var mw = new ModelWriter();
                mw.Write(opts.metadata, classes);
            }

            if (classes == null && !string.IsNullOrEmpty(opts.metadata))
            {
                var mw = new ModelWriter();
                classes = mw.Read(opts.metadata);
            }

            if (!string.IsNullOrEmpty(opts.@output) && classes != null)
            {
                var cg = new CodeGenerator();
                string code = cg.Generate(classes, opts);
                WriteSource(opts.output, code);
            }
        }

        private static void WriteSource(string path, string code)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(path, false);
                sw.Write(code);
            }
            finally
            {
                if (sw != null)
                    sw.Close();
            }
        }

        private static Options ProcessOptions(string[] args)
        {
            var opts = new Options();
            opts.ProcessArgs(args);
            return opts;
        }
    }

    public class ModelWriter
    {
        public void Write(string output, IEnumerable<OntologyClass> classes)
        {
            var serializer = new XmlSerializer(typeof (OntologyClass[]));
            var fs = new FileStream(output, FileMode.Create);
            TextWriter writer = new StreamWriter(fs, new UTF8Encoding());
            // Serialize using the XmlTextWriter.
            serializer.Serialize(writer, classes.ToArray());
            writer.Close();
        }

        public IEnumerable<OntologyClass> Read(string path)
        {
            var serializer = new XmlSerializer(typeof (OntologyClass[]));
            // Create a TextReader to read the file. 
            var fs = new FileStream(path, FileMode.OpenOrCreate);
            TextReader reader = new StreamReader(fs);
            return (OntologyClass[]) serializer.Deserialize(reader);
        }
    }

    public class ClassQuerySink : QueryResultSink
    {
        private readonly bool ignoreBnodes;
        private readonly string @namespace;
        private readonly string[] varNames;

        public List<NameValueCollection> bindings = new List<NameValueCollection>();

        public ClassQuerySink(bool ignoreBnodes, string @namespace, string[] varNames)
        {
            this.ignoreBnodes = ignoreBnodes;
            this.@namespace = @namespace;
            this.varNames = varNames;
        }

        public override bool Add(VariableBindings result)
        {
            var nvc = new NameValueCollection();

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
        public List<Uri> ls = new List<Uri>();

        #region StatementSink Members

        public bool Add(Statement s)
        {
            string output = string.Format("{0} {1} {2}", s.Subject, s.Predicate, s.Object);
            Debug.WriteLine(output);
            return true;
        }

        #endregion
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