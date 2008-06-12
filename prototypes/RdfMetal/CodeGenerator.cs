using System.Collections.Generic;
using Antlr.StringTemplate;

namespace RdfMetal
{
    public class CodeGenerator
    {
        private readonly StringTemplateGroup group = new StringTemplateGroup("myGroup", @"..\..\template");

        public string Generate(IEnumerable<OntologyClass> classes, Options opts)
        {
            StringTemplate template = group.GetInstanceOf("classes");
            template.SetAttribute("classes", classes);
            template.SetAttribute("handle", opts.handle);
            template.SetAttribute("uri", opts.@namespace);
            return template.ToString();
        }
    }
}