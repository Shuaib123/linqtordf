using System;
using System.Collections.Generic;
using System.Linq;
using SemWeb.Remote;

namespace RdfMetal
{
    internal class MetadataRetriever
    {
        public MetadataRetriever(Opts opts)
        {
            this.opts = opts;
        }
        Opts opts { get; set; }
        public IEnumerable<OntClass> GetClasses()
        {
            return GetClassUris()
                .Distinct()
                .Where(s => !string.IsNullOrEmpty(s))
                .Map(u => GetClass(u));
        }

        private IEnumerable<string> GetClassUris()
        {
            var source = new SparqlHttpSource(opts.endpoint);
            var properties = new ClassQuerySink(opts.ignoreBnodes, opts.@namespace, new[] {"u"});
            source.RunSparqlQuery(sqGetClasses, properties);
            return properties.bindings.Map(nvc => nvc["u"]);
        }

        private OntClass GetClass(string classUri)
        {
            var u = new Uri(classUri);
            var source = new SparqlHttpSource(opts.endpoint);
            var properties = new ClassQuerySink(opts.ignoreBnodes, null, new[] {"p", "r"});

            string sparqlQuery = string.Format(sqGetProperty, classUri);
            source.RunSparqlQuery(sparqlQuery, properties);
            IEnumerable<Tuple<string, string>> q1 = properties.bindings
                .Map(nvc => new Tuple<string, string>(nvc["p"], nvc["r"]))
                .Where(t => (!(string.IsNullOrEmpty(t.First) || string.IsNullOrEmpty(t.Second))));

            sparqlQuery = string.Format(sqGetObjectProperty, classUri);
            source.RunSparqlQuery(sparqlQuery, properties);
            IEnumerable<Tuple<string, string>> q2 = properties.bindings
                .Map(nvc => new Tuple<string, string>(nvc["p"], nvc["r"]))
                .Where(t => (!(string.IsNullOrEmpty(t.First) || string.IsNullOrEmpty(t.Second))));
            IEnumerable<OntProp> ops = q2.Map(t => new OntProp
                                                       {
                                                           Uri = t.First.Trim(),
                                                           IsObjectProp = true,
                                                           Name = GetNameFromUri(t.First),
                                                           Range = GetNameFromUri(t.Second)
                                                       });
            IEnumerable<OntProp> dps = q1.Map(t => new OntProp
                                                       {
                                                           Uri = t.First.Trim(),
                                                           IsObjectProp = false,
                                                           Name = GetNameFromUri(t.First),
                                                           Range = GetNameFromUri(t.Second)
                                                       });
            var d = new Dictionary<string, OntProp>();
            IEnumerable<OntProp> props = ops.Union(dps);
            foreach (OntProp prop in props)
            {
                d[prop.Uri] = prop;
            }
            var result = new OntClass
                             {
                                 Name = u.Segments[u.Segments.Length - 1],
                                 Uri = classUri,
                                 Props = d.Values.Where(p => NamespaceMatches(p)).ToArray()
                             };
            return result;
        }

        private string GetNameFromUri(string s)
        {
            var u = new Uri(s);
            if (!string.IsNullOrEmpty(u.Fragment))
            {
                return u.Fragment.Substring(1);
            }
            return u.Segments[u.Segments.Length - 1];
        }

        private bool NamespaceMatches(OntProp p)
        {
            if (string.IsNullOrEmpty(opts.@namespace))
            {
                return true;
            }
            return p.Uri.StartsWith(opts.@namespace);
        }

        #region queries

        private static string sqGetClasses =
            @"
PREFIX owl:  <http://www.w3.org/2002/07/owl#>

SELECT ?u
WHERE
	{
	?u a owl:Class .
	}
";

        private static string sqGetObjectProperty =
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

        private static string sqGetProperty =
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

        #endregion
    }

    public class OntClass
    {
        public string Uri { get; set; }
        public string Name { get; set; }
        public OntProp[] Props { get; set; }
    }

    public class OntProp
    {
        public string Uri { get; set; }
        public bool IsObjectProp { get; set; }
        public string Name { get; set; }
        public string Range { get; set; }
    }
}