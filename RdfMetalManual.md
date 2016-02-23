RdfMetal is part of the LinqToRdf tool suite. It reads an ontology in a semantic
web database and generates LinqToRdf compatible code from it. This walkthrough
shows how to use it to generate code for your project.


# Overview &amp; Usage #

RdfMetal works by querying a remote SPARQL enabled triple store to get information
about the classes defined in a user specified ontology. It stores the results in
a metadata file for use during a code generation phase. During a code generation
phase the tool will read the metadata creating C# source code from it.

The two phases of process (metadata retrieval and code generation) are
independent and can be invoked separately or together. The common thread that links
them is the metadata file stored at the end of the metadata retrieval phase.

RdfMetal takes these options:

<br><pre><code><br>
&lt;math&gt; ./RdfMetal.exe /?<br>
RdfMetal  0.1.0.0 - Copyright (c) Andrew Matthews 2008<br>
A code generator for LinqToRdf<br>
<br>
Usage: RdfMetal [options] is used with the following options<br>
Options:<br>
-e -endpoint:PARAM      The SPARQL endpoint to query.<br>
-h -handle:PARAM        The ontology name to be used in<br>
LinqToRdf for prefixing URIs and<br>
disambiguating class names and<br>
properties.<br>
-? -help                Show this help list<br>
-help2               Show an additional help list<br>
-i -ignorebnodes        Ignore BNodes. Use this if you only<br>
want to generate<br>
code for named classes.<br>
-m -metadata:PARAM      Where to place/get the collected<br>
metadata.<br>
-n -namespace:PARAM     The XML Namespace to extract classes<br>
from.<br>
-N -netnamespace:PARAM  The .NET namespace to place the<br>
generated source in.<br>
-o -output:PARAM        Where to place the generated code.<br>
-r -references:PARAM    A comma separated list of namespaces to<br>
reference in the generated source.<br>
-usage               Show usage syntax and exit<br>
-V -version             Display version and licensing informa-<br>
tion<br>
</code></pre><br>

If you provide a SPARQL endpoint, RdfMetal will connect to it and retrieve<br>
whatever domain model it can find there. If you provide a filename for the<br>
metadata to be stored in, it will save the metadata as XML in that file. If you<br>
want you can stop at that point and generate code later. If you don't provide a<br>
metadata storage location the data will be passed directly to the code<br>
generation part of the application, that will generate source from it. To generate<br>
source, you must provide an output location for the source. If no output<br>
location is provided, then no source is created and the application stops.<br>
<br>
The steps you need to follow are:<br>
<br>
\begin{list}{-}<br>
Locate the SPARQL endpoint where the data is stored<br>
Restrict the ontology namespace URI to the object model you are interested in.<br>
Invoke RdfMetal to retrieve the class definitions from the remote SPARQL endpoint.<br>
Add the source RdfMetal generates to your project<br>
Reference the =LinqToRdf.dll=  assembly in your project<br>
Query the object model using LINQ.<br>
\end{list}<br>
<br>
\section<b>{Walkthrough}</b>

<h2>Installing RdfMetal</h2>
RdfMetal is part of the LinqToRdf tool suite. If you have the latest version of<br>
LinqToRdf, then you should have a copy. If not, you should download and install<br>
the latest featured release of LinqToRdf at<br>
=<a href='http://code.google.com/p/='>http://code.google.com/p/=</a>  =linqtordf= . After installing LinqToRdf you<br>
should have a program files directory at \texttt{c:/Program Files/Andrew<br>
Matthews} =/LinqToRdf= .  Inside which is all that you need to be able to run RdfMetal.<br>
It's a command line tool, so you will need to either put that directory in the<br>
path, or refer to the RdfMetal executable using the full path, as we'll do in this<br>
walkthrough.<br>
<br>
<h2>Finding the endpoint URI</h2>
The first thing you will require is the SPARQL endpoint URL of the semantic web<br>
database you're planning to work with. For this walkthrough, we'll be using the<br>
sample triple store that is used by the LinqToRdf project for testing. In this<br>
case, you query it through the URL<br>
=<a href='http://localhost/linqtordf='>http://localhost/linqtordf=</a>  =/SparqlQuery.aspx= . You can find out more<br>
about hosting your own ontologies in the LinqToRdf manual, or in my article<br>
Understanding SPARQL.<br>
<br>
You need to know this endpoint URL because RdfMetal will send a series of<br>
SPARQL queries to the database requesting information about the classes stored<br>
there and their properties and relationships.<br>
<br>
<h2>Create a pre-build step in your project</h2>

In this walkthrough, we use RdfMetal to generate code as part of a pre-build<br>
step. That means the metadata will be retrieved and used to generate source<br>
code every time your project gets built. This probably overkill, especially if<br>
the remote ontology is not subject to frequent changes. In that case you might<br>
want to perform these steps manually as required.<br>
<br>
This command line invokes RdfMetal using the build step macro \<br>
<br>
Unknown end tag for </math><br>
<br>
(ProjectDir)<br>
defined in Visual Studio.  Typically, you define the SPARQL endpoint as an<br>
initial phase, and then cache metadata gathered in an XML file. Once the<br>
metadata has been gathered, C# source code can be generated from it. If the<br>
target ontology is static, you may choose not to continue using RdfMetal after the<br>
initial source code generation. Here, we're simply gathering the metadata every<br>
time and storing it in a source file called =music.cs= .<br>
<br>
<br><pre><code><br>
"c:\Program Files\Andrew Matthews\RdfMetal\RdfMetal.exe"<br>
-e:http://localhost/linqtordf/SparqlQuery.aspx<br>
-i -n http://aabs.purl.org/ontologies/2007/04/music\#<br>
-o "&lt;math&gt;(ProjectDir)music.cs" -N:RdfMetal.Music -h music<br>
</code></pre><br>

<h1>Using RdfMetal with Visual Studio</h1>
RdfMetal  was designed to used from a pre-build event in Visual Studio. The example<br>
above, shows how it can be used to generate source for the small ontology used<br>
to test LinqToRdf.  It queries a local website called =LinqToRdf= . (You<br>
can get instructions on how to set up such a self hosted ontology from the<br>
LinqToRdf manual.) I'm restricting the classes to only those defined in the<br>
=<a href='http://aabs.purl.org/ontologies/2007/04/music\#='>http://aabs.purl.org/ontologies/2007/04/music\#=</a>  namespace and am<br>
generating code in the =RdfMetal.Music=  .NET namespace. The internal name<br>
I'm using is '=music= ', and that's the preferred prefix to be used in any generated RDF or SPARQL.<br>
<br>
When RdfMetal is run it outputs a list of the classes that it is generating source<br>
for, then writes 'done'. Here's some output from the music ontology:<br>
<br>
<br><pre><code><br>
------ Build started: Project: RdfMetalTestHarness<br>
C:\. . .\linqtordf\src\RdfMetal\bin\Debug\RdfMetal.exe<br>
-e:http://localhost/LINQTORDF/SparqlQuery.aspx -i -n<br>
http://aabs.purl.org/ontologies/2007/04/music\# -o<br>
"C:\. . .\linqtordf\src\unit-testing\<br>
RdfMetalTestHarness\music.xml" -N:RdfMetal.Music -h music<br>
<br>
ProducerOfMusic<br>
SellerOfMusic<br>
NamedThing<br>
TemporalThing<br>
Person<br>
Band<br>
Studio<br>
Music<br>
Album<br>
Track<br>
Song<br>
Mp3File<br>
Genre<br>
done.<br>
</code></pre><br>

Each of these classes is defined in the =store.n3=  file in the unit tests<br>
directory in the LinqToRdf solution. The source that it generates will be in the<br>
file =music.cs= . The output is too long and repetitive to be worth including in<br>
full, but here's some edited highlights. The generator creates a =DataContext=<br>
class containing standard query properties for each of the class types found in<br>
the metadata extraction process. In this case the queries are included for<br>
<h1>Album=  and =Track</h1>

<br><pre><code><br>
public class musicDataContext : RdfDataContext<br>
{<br>
public musicDataContext(TripleStore store)<br>
: base(store)<br>
{<br>
}<br>
<br>
public musicDataContext(string store)<br>
: base(new TripleStore(store))<br>
{<br>
}<br>
<br>
public IQueryable&lt;Album&gt; Albums<br>
{<br>
get { return ForType&lt;Album&gt;(); }<br>
}<br>
<br>
public IQueryable&lt;Track&gt; Tracks<br>
{<br>
get { return ForType&lt;Track&gt;(); }<br>
}<br>
<br>
</code></pre><br>

In most cases the classes generated are empty, but in the test data the Track<br>
and Album classes have several DatatypeProperties as well as ObjectProperties.<br>
Here's the code generated for the =Track=  class.<br>
<br>
<br><pre><code><br>
[OwlResource(OntologyName = "music",<br>
RelativeUriReference = "Track")]<br>
public class Track : OwlInstanceSupertype<br>
{<br>
#region Datatype properties<br>
<br>
[OwlResource(OntologyName = "music",<br>
RelativeUriReference = "title")]<br>
public string title { get; set; } // Track<br>
[OwlResource(OntologyName = "music",<br>
RelativeUriReference = "artistName")]<br>
public string artistName { get; set; } // Track<br>
[OwlResource(OntologyName = "music",<br>
RelativeUriReference = "albumName")]<br>
public string albumName { get; set; } // Track<br>
[OwlResource(OntologyName = "music",<br>
RelativeUriReference = "genreName")]<br>
public string genreName { get; set; } // Track<br>
[OwlResource(OntologyName = "music",<br>
RelativeUriReference = "comment")]<br>
public string comment { get; set; } // Track<br>
[OwlResource(OntologyName = "music",<br>
RelativeUriReference = "fileLocation")]<br>
public string fileLocation { get; set; } // Track<br>
<br>
#endregion<br>
<br>
#region Incoming relationships properties<br>
<br>
#endregion<br>
<br>
#region Object properties<br>
<br>
[OwlResource(OntologyName = "music",<br>
RelativeUriReference = "isTrackOn")]<br>
public string isTrackOnUri { get; set; }<br>
<br>
private EntityRef&lt;Album&gt; _isTrackOn { get; set; }<br>
<br>
[OwlResource(OntologyName = "music",<br>
RelativeUriReference = "isTrackOn")]<br>
public Album isTrackOn<br>
{<br>
get<br>
{<br>
if (_isTrackOn.HasLoadedOrAssignedValue)<br>
return _isTrackOn.Entity;<br>
if (DataContext != null)<br>
{<br>
var ctx = (musicDataContext) DataContext;<br>
_isTrackOn = new EntityRef&lt;Album&gt;(<br>
from x in ctx.Albums<br>
where x.HasInstanceUri(isTrackOnUri)<br>
select x);<br>
return _isTrackOn.Entity;<br>
}<br>
return null;<br>
}<br>
}<br>
<br>
#endregion<br>
}<br>
<br>
</code></pre><br>

Note the use of the =EntityRef=  in the =<i>isTrackOn=  field and the<br>
=isTrackOn=  property to provide navigation across the object graph.</i>

<br><pre><code><br>
[OwlResource(OntologyName = "Music",<br>
RelativeUriReference = "Album")]<br>
public class Album : OwlInstanceSupertype<br>
{<br>
public Album()<br>
{<br>
}<br>
<br>
[OwlResource(OntologyName = "Music",<br>
RelativeUriReference="name")]<br>
public string Name { get; set; }<br>
<br>
private EntitySet&lt;Track&gt; _Tracks = new EntitySet&lt;Track&gt;();<br>
[OwlResource(OntologyName = "Music",<br>
RelativeUriReference = "isTrackOn")]<br>
public EntitySet&lt;Track&gt; Tracks<br>
{<br>
get<br>
{<br>
if (_Tracks.HasLoadedOrAssignedValues)<br>
return _Tracks;<br>
if (DataContext != null)<br>
{<br>
_Tracks.SetSource(from t in<br>
((MusicDataContext)DataContext).Tracks<br>
where t.AlbumName == Name<br>
select t);<br>
}<br>
return _Tracks;<br>
}<br>
}<br>
}<br>
<br>
</code></pre><br>

Again notice the use of =EntitySet= s to provide navigation. This time the<br>
navigation is from parent to child.<br>
<br>
Once the code is generated all you need to do is incorporate it into your<br>
project and use it like any other LinqToRdf code. For instructions on how to do<br>
that, consult the LinqToRdf manual for guidance.<br>
<br>
