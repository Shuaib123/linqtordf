using Opts = Mono.GetOptions.Options;
using Opt = Mono.GetOptions.OptionAttribute;

namespace RdfMetal
{
    public class Options : Opts
    {
        [Opt("The SPARQL endpoint to query.", 'e', "endpoint")]
        public string endpoint = "";

        [Opt("The XML Namespace to extract classes from.", 'n', "namespace")]
        public string @namespace = "MyOntology";

        [Opt("Where to place the generated code.", 'o', "output")]
        public string @output = "DomainModel.cs";

        [Opt("Where to place/get the collected metadata.", 'm', "metadata")]
        public string @metadata = "";

        [Opt("Ignore BNodes. Use this if you only want to generate code for named classes.", 'i', "ignorebnodes")]
        public bool ignoreBnodes = false;

        [Opt("The ontology name to be used in LinqToRdf for prefixing URIs and disambiguating class names and properties.", 'h', "handle")]
        public string handle = "MyOntology";

        public Options()
        {
            base.ParsingMode = Mono.GetOptions.OptionsParsingMode.Both;
        }

    }
}