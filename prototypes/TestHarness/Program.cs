using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqToRdf;

namespace TestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
namespace RdfMetal.Time
{
    [OwlResource(OntologyName = "time", RelativeUriReference = "TemporalEntity")]
    public partial class TemporalEntity : OwlInstanceSupertype
    {
        #region Datatype properties
        #endregion

        #region Incoming relationships properties
        #endregion

        #region Object properties
        #endregion
    }

}

/*
 * 
 * -e:http://DBpedia.org/sparql
 * 
$(SolutionDir)$(OutDir)RdfMetal.exe -i -n http://purl.org/vocab/frbr/core# -o "C:\shared.datastore\repository\personal\dev\projects\semantic-web\LinqToRdf.Prototypes\TestHarness\frbr.cs" -m "C:\shared.datastore\repository\personal\dev\projects\semantic-web\LinqToRdf.Prototypes\TestHarness\frbr.xml" -N:RdfMetal.Frbr -h frbr
$(SolutionDir)$(OutDir)RdfMetal.exe -i -n http://xmlns.com/foaf/0.1/ -o "C:\shared.datastore\repository\personal\dev\projects\semantic-web\LinqToRdf.Prototypes\TestHarness\foaf.cs" -m "C:\shared.datastore\repository\personal\dev\projects\semantic-web\LinqToRdf.Prototypes\TestHarness\foaf.xml" -N:RdfMetal.Foaf -h foaf
$(SolutionDir)$(OutDir)RdfMetal.exe -i -n http://www.w3.org/2006/time# -o "C:\shared.datastore\repository\personal\dev\projects\semantic-web\LinqToRdf.Prototypes\TestHarness\time.cs" -m "C:\shared.datastore\repository\personal\dev\projects\semantic-web\LinqToRdf.Prototypes\TestHarness\time.xml" -N:RdfMetal.Time -h time
$(SolutionDir)$(OutDir)RdfMetal.exe -i -n http://www.w3.org/2003/01/geo/wgs84_pos# -o "C:\shared.datastore\repository\personal\dev\projects\semantic-web\LinqToRdf.Prototypes\TestHarness\space.cs" -m "C:\shared.datastore\repository\personal\dev\projects\semantic-web\LinqToRdf.Prototypes\TestHarness\space.xml" -N:RdfMetal.Space -h space
$(SolutionDir)$(OutDir)RdfMetal.exe -i -n http://purl.org/ontology/mo/ -o "C:\shared.datastore\repository\personal\dev\projects\semantic-web\LinqToRdf.Prototypes\TestHarness\mo.cs" -m "C:\shared.datastore\repository\personal\dev\projects\semantic-web\LinqToRdf.Prototypes\TestHarness\meta.xml" -N:RdfMetal.Music -r:RdfMetal.Foaf,RdfMetal.Time,RdfMetal.Space,RdfMetal.Frbr -h music
*/