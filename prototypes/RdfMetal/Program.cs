using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

/*
 * -i -n http://xmlns.com/foaf/0.1/ -o ..\..\out.cs -m ..\..\meta.xml
 * -e:http://DBpedia.org/sparql -i -n http://xmlns.com/foaf/0.1/ -o ..\..\out.cs -m ..\..\meta.xml
 * */
namespace RdfMetal
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Options opts = ProcessOptions(args);
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
                AnnotateClasses(classes);
                ProcessClassRelationships(classes);
                var cg = new CodeGenerator();
                string code = cg.Generate(classes, opts);
                WriteSource(opts.output, code);
            }
	    Console.WriteLine("done.");
            Console.ReadKey();
        }

        private static void AnnotateClasses(IEnumerable<OntologyClass> classes)
        {
            foreach (var c in classes)
            {
                foreach (var p in c.Properties)
                {
                    p.HostClass = c;
                }
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

        public static void ProcessClassRelationships(IEnumerable<OntologyClass> classes)
        {
            foreach (var ontCls in classes)
            {
                ontCls.IncomingRelationships = classes
                    .Map(c => c.OutgoingRelationships.AsEnumerable())
                    .Flatten()
                    .Where(p=>p.Range == ontCls.Name)
                    .ToArray(); 
            }
        }
    }
}
