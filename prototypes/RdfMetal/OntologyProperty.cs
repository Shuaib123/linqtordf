namespace RdfMetal
{
    public class OntologyProperty
    {
        public string Uri { get; set; }
        public bool IsObjectProp { get; set; }
        public string Name { get; set; }
        public string Range { get; set; }
        public OntologyClass HostClass { get; set; }
    }
}
