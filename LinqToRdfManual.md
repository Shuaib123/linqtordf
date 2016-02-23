This document guides you through the steps needed to write a simple semantic
web application in C#. It covers the tools and systems prerequisites, the
techniques and the expected behavior. All examples are in C#, although
LinqToRdf should work on any .NET language that supports LINQ.

# Introduction #
## What are the components of a Semantic Web Application? ##
Within the context of this document, `Semantic Web Application' means
"application that uses RDF and related technologies". That is -- an
application that represents information as a graph structure using RDF. It�s
beyond the scope of this document to explain the whole pyramid of standards and
technologies needed to support the semantic web. Instead I�ll give context
enough for you to know what steps are required to get your Semantic Web
Application off the ground.



\begin{figure}
\centering
\includegraphics[width=0.5\textwidth]{images/img1}
\caption{Major technologies employed with LinqToRdf.}
\end{figure}
LinqToRdf uses the SemWeb.NET framework by Joshua Tauberer, which provides a
platform for working with OWL and SPARQL. It also uses the .NET 3.5 namespace
=System.Linq=  which will be released as part of Visual Studio .NET 2008.

\begin{figure}
\centering
\includegraphics{images/img2}
\caption{The hierarchy of technologies used in the semantic web.}
\end{figure}
# Programming with LinqToRdf #

## What do you need to do semantic web programming with LinqToRdf? ##
LinqToRdf requires a runtime that supports the latest version of LINQ, which at
the time of writing is .NET 3.5 or Mono 1.9. LinqToRdf has not been tested with
Mono yet, but since Mono 1.9 now supports C# it should be able to run.

## Where to get LinqToRdf ##
You can download the latest release from Google code (latest version at time of
writing is LinqToRdf-0.8.msi) . That installer also contains an installer for te
LinqToRdf designer - an extension to visual studio 2008 that will provide a
simple UML design surface for producing ontologies and domain models. Check the
linqtordf-discuss discussion forum for announcements of newer releases.

## Installation procedure ##
Installation is a simple matter of double clicking the MSI file, and deciding
where to install the assemblies for LinqToRdf. The default location will be in
a =LinqToRdf-0.x=  directory under ``
> \verb|c:\Program Files\Andrew Matthews|''.
Once LinqToRdf is installed, you should then install LinqToRdfDesigner, which
will register the DSL (Domain Specific Language) add-in to Visual Studio. Note
that LinqToRdf and LinqToRdfDesigner will only work with Visual Studio 2008 and
above.

## Creating an ontology ##
The details of the OWL standard are beyond the scope of this document. The
standards document is found at the W3C . There are various tools available for
creating RDF, and it is important to know that the SemWeb library can
understand the Notation 3 syntax  , which is a human readable (non-XML) variant
of RDF. The examples we�ll be using in the rest of this document use Notation 3
(or N3 for short). If your requirements are modest the LinqToRdf graphical
designer should be sufficient for most tasks. If you need to create more
complex ontologies, or if you need them to be created in OWL, RDF or RDFS
format, then you might want to consider tools such as Prot�g�.
LinqToRdfDesigner will be covered in a later section. This section shows how
the LinqToRdf framework can be used without any other support.  Let�s start
with a simple ontology for recording MP3 files. The ontology file should have
an extension of n3. Let�s call ours music.n3. First you define the XML
namespaces you will be working with:

<br><pre><code><br>
@prefix rdf:  &lt;http://www.w3.org/1999/02/22-rdf-syntax-ns\#&gt; .<br>
@prefix daml: &lt;http://www.daml.org/2001/03/daml+oil\#&gt; .<br>
@prefix log: &lt;http://www.w3.org/2000/10/swap/log\#&gt; .<br>
@prefix rdfs: &lt;http://www.w3.org/2000/01/rdf-schema\#&gt; .<br>
@prefix owl:  &lt;http://www.w3.org/2002/07/owl\#&gt; .<br>
@prefix xsdt: &lt;http://www.w3.org/2001/XMLSchema\#&gt;.<br>
@prefix : &lt;http://aabs.purl.org/ontologies/2007/04/music\#&gt; .<br>
<br>
</code></pre><br>

This imports some of the standard namespaces for OWL, RDF, XML Schema datatypes<br>
and others. It also defines a default namespace to be used for all classes and<br>
properties that are going to be defined in the rest of the document. Now let�s<br>
define some classes:<br>
<br>
<br><pre><code><br>
:Album a owl:Class.<br>
:Track a owl:Class.<br>
:title rdfs:domain :Track;<br>
rdfs:range  xsdt:string.<br>
:artistName<br>
rdfs:domain :Track;<br>
rdfs:range  xsdt:string.<br>
:albumName<br>
rdfs:domain :Track;<br>
rdfs:range  xsdt:string.<br>
:year<br>
rdfs:domain :Album;<br>
rdfs:range  xsdt:integer.<br>
:genreName<br>
rdfs:domain :Track;<br>
rdfs:range  xsdt:string.<br>
:comment<br>
rdfs:domain :Track;<br>
rdfs:range  xsdt:string.<br>
:isTrackOn<br>
rdfs:domain :Track;<br>
rdfs:range  :Album.<br>
:fileLocation<br>
rdfs:domain :Track;<br>
rdfs:range  xsdt:string.<br>
<br>
</code></pre><br>

This defines a class Track of type owl:Class. After the class declaration, I<br>
defined some properties on the Track Class (:title, :artistName etc). Because<br>
the prolog section previously defined a default namespace, these declarations<br>
are now in the <<a href='http://aabs.purl.org/ontologies/2007/04/music\#>'>http://aabs.purl.org/ontologies/2007/04/music\#&gt;</a> namespace.<br>
That�s all that�s required to define our simple ontology. Now we can to create<br>
some data for MP3 files.<br>
<br>
Create another file called mp3s.n3 and add the following:<br>
<br><pre><code><br>
@prefix ns1: &lt;http://aabs.purl.org/ontologies/2007/04/music\#&gt; .<br>
ns1:Track_-861912094 &lt;http://www.w3.org/1999/02/22-rdf-syntax-ns\#type&gt; ns1:Track ;<br>
ns1:title "History 5 | Fall 2006 | UC Berkeley" ;<br>
ns1:artistName "Thomas Laqueur" ;<br>
ns1:albumName "History 5 | Fall 2006 | UC Berkeley" ;<br>
ns1:year "2006" ;<br>
ns1:genreName "History 5 | Fall 2006 | UC Berkeley" ;<br>
ns1:comment " (C) Copyright 2006, UC Regents" ;<br>
ns1:isTrackOn ns1:Album_2 ;<br>
ns1:fileLocation "C:\\Users\\andrew.matthews\\Music\\hist5_20060829.mp3" .<br>
ns1:Track_-1378138934 &lt;http://www.w3.org/1999/02/22-rdf-syntax-ns\#type&gt; ns1:Track ;<br>
ns1:title "History 5 | Fall 2006 | UC Berkeley" ;<br>
ns1:artistName "Thomas Laqueur" ;<br>
ns1:albumName "History 5 | Fall 2006 | UC Berkeley" ;<br>
ns1:year "2006" ;<br>
ns1:genreName "History 5 | Fall 2006 | UC Berkeley" ;<br>
ns1:comment " (C) Copyright 2006, UC Regents" ;<br>
ns1:isTrackOn ns1:Album_2 ;<br>
ns1:fileLocation "C:\\Users\\andrew.matthews\\Music\\hist5_20060831.mp3" .<br>
ns1:Track_583675819 &lt;http://www.w3.org/1999/02/22-rdf-syntax-ns\#type&gt; ns1:Track ;<br>
ns1:title "Rory Blyth: The Smartest Man in the World\u0000" ;<br>
ns1:artistName "Rory Blyth\u0000" ;<br>
ns1:albumName "Rory Blyth: The Smartest Man in the World\u0000" ;<br>
ns1:year "2007\u0000" ;<br>
ns1:genreName "Rory Blyth: The Smartest Man in the World\u0000" ;<br>
ns1:comment "Einstein couldn't do it again if he lived today. He'd be<br>
too distracted by the allure of technology, and by all those buttheads<br>
at Mensa trying to prove how smart they are." ;<br>
ns1:isTrackOn ns1:Album_2 ;<br>
ns1:fileLocation "C:\\Users\\andrew.matthews\\Music\\iTunes\\iTunes<br>
Music\\Podcasts\\Rory Blyth_ The Smartest Man in the Worl\\A Few<br>
Thoughts on the Subject of Gen.mp3" .<br>
<br>
</code></pre><br>

These entries were taken randomly from a list of podcasts that I subscribe to.<br>
In addition, I wrote a program to create them, but you could do it by hand if<br>
you want to. I�ll show you in a little while how I created the entries in the<br>
=mp3s.n3=  file. In mp3s.n3 I defined a namespace called ns1. It refers to what<br>
was the default namespace in music.n3.  That means that references to entities<br>
from the ontology like �Track� will be called �ns1:Track� rather than �:Track�<br>
as they were called in =music.n3= .  It doesn�t matter what you call the prefix<br>
for the namespace, just so long as the URI that it maps to is the same as was<br>
used in the ontology definition file. I called it =ns1= , because that�s what my<br>
import program wanted to do. The point is that the type =ns1:Track=  in this file<br>
refers to the =:Track=  class defined in =music.n3= . The triple store that we�ll get<br>
to shortly will be able to make sense of that in order to know that a ns1:Track<br>
has a title, artist etc. It is also able to work out the types of the<br>
properties (which just happens to be �string� for the moment).<br>
<br>
That�s it. That�s all there is to creating an ontology. Later on, we�ll get<br>
onto the more complicated task of linking types together using<br>
ObjectProperties, but for now you have an ontology and some data that uses it.<br>
<br>
<h2>Hosting your ontology</h2>
Since the uptake of semantic web technologies has been pretty patchy in the<br>
.NET domain so far, your best bet for industrial strength RDF triple stores will (for<br>
now) be either Jena (=jena.sf.net= ) or OpenLink<br>
(=openlinksw.com= ), though there are various triple store solutions that<br>
can be used. For this guide I shall stick to .NET by using Joshua Tauberer�s<br>
SPARQL enabled HttpHandler for ASP.NET, which is sufficient to demonstrate how<br>
LinqToRdf can connect to a SPARQL compatible triple store. LinqToRdf has been<br>
tested with the Joseki SPARQL interface to Jena, and the OpenLink Virtuoso<br>
platform. LinqToRdf is platform independent in the sense that it will happily<br>
work on any standards compliant SPARQL server.<br>
<br>
To use the HttpHandler as a triple store for music.n3, create an ASP.NET<br>
application in visual studio. Place the following into configuration section of<br>
the web.config of the project:<br>
<br>
<br><pre><code><br>
&lt;configSections&gt;<br>
&lt;section name="sparqlSources"<br>
type="System.Configuration.NameValueSectionHandler, System,<br>
Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"/&gt;<br>
<br>
<br>
Unknown end tag for &lt;/configSections&gt;<br>
<br>
<br>
<br>
</code></pre><br>

Next, add the following to the body of the body of the config file beneath the root:<br>
<br><pre><code><br>
&lt;sparqlSources&gt;<br>
&lt;add key="/[your vdir here]/SparqlQuery.aspx"<br>
value="n3:[your path here]\mp3s.n3"/&gt;<br>
<br>
<br>
Unknown end tag for &lt;/sparqlSources&gt;<br>
<br>
<br>
<br>
</code></pre><br>

The name SparqlQuery.aspx doesn�t matter � it doesn�t exist. You can call it<br>
whatever you want. What this does is link the URL for the file SparqlQuery.aspx<br>
to the SPARQL HttpHandler that we next add to the system.web section of the<br>
web.config:<br>
<br>
<br><pre><code><br>
&lt;httpHandlers&gt;<br>
&lt;!-- This line associates the SPARQL Protocol implementation with a path on<br>
your website. With this, you get a SPARQL server at<br>
http://yourdomain.com/sparql.  --&gt;<br>
&lt;add verb="*" path="SparqlQuery.aspx"<br>
type="SemWeb.Query.SparqlProtocolServerHandler, SemWeb.Sparql" /&gt;<br>
<br>
<br>
Unknown end tag for &lt;/httpHandlers&gt;<br>
<br>
<br>
<br>
</code></pre><br>

This uses the HttpHandler defined in SemWeb to accept SPARQL queries and run<br>
them against the triples defined in mp3s.n3. That�s all that�s needed to turn<br>
your ASP.NET into a semantic web triple store! Yes, it�s that easy. To use the<br>
HttpHandler you give the URL (/<a href='your.md'>vdir here</a>/SparqlQuery.aspx in the example<br>
above) defined above in a TripleStore object that is passed to the RDF context<br>
object. Here�s an example taken from the LinqToRdf test suite.<br>
<br>
<br><pre><code><br>
TripleStore ts = new TripleStore();<br>
ts.EndpointUri = @"http://localhost/linqtordf/SparqlQuery.aspx";<br>
ts.QueryType = QueryType.RemoteSparqlStore;<br>
<br>
</code></pre><br>

the LinqToRdf SparqlQuery object will use this to direct queries via HTTP to<br>
the triple store located at ts.EndpointUri.<br>
<br>
<h2>Linking to the ontology from .NET</h2>

Now we have an ontology defined, and somewhere to host it that understands<br>
SPARQL we can start using LinqToRdf. References to ontologies are defined at<br>
the assembly level to prevent repetition (as was the case in earlier versions<br>
of LinqToRdf). You create an Ontology Attribute for each of the ontologies that<br>
are referenced in the application or ontology. Below is an example from the<br>
SystemScanner reference application.<br>
<br>
<br><pre><code><br>
[assembly: Ontology(<br>
BaseUri = "http://aabs.purl.org/ontologies/2007/10/"<br>
+"system-scanner\#",<br>
Name = "SystemScanner",<br>
Prefix = "syscan",<br>
UrlOfOntology =<br>
"file:///C:/etc/dev/semantic-web/linqtordf/doc/"<br>
+"Samples/SystemScanner/rdf/sys.n3")]<br>
[assembly: Ontology(<br>
Prefix = "rdf",<br>
BaseUri = "http://www.w3.org/1999/02/22-rdf-syntax-"<br>
+"generatedNamespaceChar\#",<br>
Name = "RDF")]<br>
[assembly: Ontology(<br>
Prefix = "rdfs",<br>
BaseUri = "http://www.w3.org/2000/01/rdf-schema\#",<br>
Name = "RDFS")]<br>
[assembly: Ontology(<br>
Prefix = "xsdt",<br>
BaseUri = "http://www.w3.org/2001/XMLSchema\#",<br>
Name = "Data Types")]<br>
[assembly: Ontology(<br>
Prefix = "fn",<br>
BaseUri = "http://www.w3.org/2005/xpath-functions\#",<br>
Name = "XPath Functions")]<br>
<br>
</code></pre><br>

Each ontology has a number of named properties that can be set. In the example<br>
above, we have only set all of the properties for the system-scanner ontology<br>
itself. The others are standard namespaces that are well-known to the<br>
underlying components of LinqToRdf. They only require a prefix, BaseUri and<br>
Name property to be set. The Prefix property is a suggestion only to LinqToRdf<br>
of how you want the namespaces to be referenced. If LinqToRdf finds a clash<br>
between prefixes, then it will make a prefix name up instead.<br>
<br>
The BaseUri is the fully qualified BaseUri used to form full resource URIs<br>
within the triple store. The Name property is used internally to refer to the<br>
ontology symbolically. The other attribute OwlResource has an OntologyName<br>
property that refers to the name of the ontology defined at the assembly level.<br>
Since the prefix is subject to change without notice, the Name property is used<br>
instead to embed a class, field or property into a specific named ontology.<br>
<br>
Next you should create a new class called Track in a file called Track.cs.<br>
We�ll see an easier way to do this later on, but for now we�re going to do it<br>
the hard way.<br>
<br>
<br><pre><code><br>
using LinqToRdf;<br>
namespace RdfMusic<br>
{<br>
[OwlResource(OntologyName="Music", RelativeUriReference="Track")]<br>
public class Track : OwlInstanceSupertype<br>
{<br>
<br>
[OwlResource(OntologyName = "Music",<br>
RelativeUriReference = "title")]<br>
public string Title { get; set; }<br>
<br>
[OwlResource(OntologyName = "Music",<br>
RelativeUriReference="artistName")]<br>
public string ArtistName { get; set; }<br>
<br>
[OwlResource(OntologyName = "Music",<br>
RelativeUriReference="albumName")]<br>
public string AlbumName { get; set; }<br>
<br>
[OwlResource(OntologyName = "Music",<br>
RelativeUriReference="year")]<br>
public string Year { get; set; }<br>
<br>
[OwlResource(OntologyName = "Music",<br>
RelativeUriReference="genreName")]<br>
public string GenreName { get; set; }<br>
<br>
[OwlResource(OntologyName = "Music",<br>
RelativeUriReference="comment")]<br>
public string Comment { get; set; }<br>
<br>
[OwlResource(OntologyName = "Music",<br>
RelativeUriReference="fileLocation")]<br>
public string FileLocation { get; set; }<br>
<br>
[OwlResource(OntologyName = "Music",<br>
RelativeUriReference="rating")]<br>
public int Rating { get; set; }<br>
<br>
public Track(TagHandler th, string fileLocation)<br>
{<br>
FileLocation = fileLocation;<br>
Title = th.Track;<br>
ArtistName = th.Artist;<br>
AlbumName = th.Album;<br>
Year = th.Year;<br>
GenreName = th.Genere;<br>
Comment = th.Comment;<br>
}<br>
<br>
private EntityRef&lt;Album&gt; _Album { get; set; }<br>
[OwlResource(OntologyName = "Music",<br>
RelativeUriReference = "isTrackOn")]<br>
public Album Album<br>
{<br>
get<br>
{<br>
if (_Album.HasLoadedOrAssignedValue)<br>
return _Album.Entity;<br>
if (DataContext != null)<br>
{<br>
var ctx = (MusicDataContext)DataContext;<br>
string trackUri = this.InstanceUri;<br>
string trackPredicateUri =<br>
this.PredicateUriForProperty(MethodBase.GetCurrentMethod());<br>
_Album = new EntityRef&lt;Album&gt;(<br>
from r in ((MusicDataContext)DataContext).Albums<br>
where r.StmtObjectWithSubjectAndPredicate(trackUri,<br>
trackPredicateUri)<br>
select r);<br>
<br>
return _Album.Entity;<br>
}<br>
return null;<br>
}<br>
}<br>
public Track()<br>
{<br>
<br>
}<br>
}<br>
</code></pre><br>

The class is just the same as any other entity class except that the class and<br>
its properties have been annotated with OwlResource attributes . The critical<br>
bit to get right is to use the same URI in =OntologyAttribute=  as we used in<br>
=music.n3=  and =mp3s.n3=  for the namespace definitions. Using<br>
<h1>OntologyAttribute</h1>
plus =OntologyName=  allows you to define all attributes as relative URIs which<br>
makes for a much more readable source file. The =OwlResourceAttribute=  defines<br>
our .NET class �=RdfMusic.Track= � to correspond with the OWL class<br>
=<a href='http://aabs.purl.org/ontologies/2007/04/music\#Track='>http://aabs.purl.org/ontologies/2007/04/music\#Track=</a> . Likewise the<br>
�=FileLocation= � property defined on it corresponds to the RDF datatype property<br>
=<a href='http://aabs.purl.org/ontologies/2007/04/music\#fileLocation='>http://aabs.purl.org/ontologies/2007/04/music\#fileLocation=</a> . The Boolean true on<br>
these attributes simply tells LinqToRdf that the URIs are relative. It then<br>
knows enough to be able to work out how to query for the details needed to fill<br>
each of the properties on the class Track.<br>
<br>
This approach is deliberately as close as possible to LINQ to SQL. It is hoped<br>
that those who are already familiar with DLINQ (as LINQ to SQL used to be<br>
known) will be able to pick this up and start working with it quickly. In<br>
DLINQ, instead of URIs for resources defined in an ontology, you would find<br>
table and column names.<br>
<br>
<br>
That�s all you need to be able to model your ontology classes in .NET. Now we<br>
can move on to the techniques needed to query your RDF triple store.<br>
<br>
<h2>Querying the ontology using SPARQL</h2>

The steps to start making queries are also pretty simple. First just create a<br>
simple LINQ enabled console application called =MyRdfTest= . Open up<br>
=Program.cs=  up<br>
for editing, and add namespace import statements for =System.Linq= ,<br>
=LinqToRdf=  and =SemWeb= :<br>
<br>
<br><pre><code><br>
using System;<br>
using LinqToRdf;<br>
using System.Linq;<br>
<br>
</code></pre><br>

In =Main= , create a =TripleStore=  object with the location of the SPARQL server:<br>
<br>
<br><pre><code><br>
private static void Main(string[] args)<br>
{<br>
TripleStore ts = new TripleStore();<br>
ts.EndpointUri = @"http://localhost/linqtordf/SparqlQuery.aspx";<br>
ts.QueryType = QueryType.RemoteSparqlStore;<br>
<br>
</code></pre><br>

=TripleStore=  is used to carry any information needed about the triple store for<br>
later use by the query. In this case I set up an IIS virtual directory on my<br>
local machine called =linqtordf= , and followed the steps outlined early. The<br>
=QueryType=  just indicates to the query context that we will be using SPARQL over<br>
HTTP. That tells it what types of connections, commands, XML data types and the<br>
query language to use.<br>
<br>
Now we�re ready to perform the LINQ query. We�ll get all of the tracks from<br>
2007 that have a genre name of ``\textsf{Rory Blyth: The Smartest Man in the<br>
World}''.<br>
We�ll create a new anonymous type to store the results in, and we�re only<br>
interested in  the =Title=  and the =FileLocation= .<br>
<br>
<br><pre><code><br>
var q = from t in new RDF(ts).ForType&lt;MyTrack&gt;()<br>
where t.Year == "2007" &amp;&amp;<br>
t.GenreName == "Rory Blyth: The Smartest Man in the World"<br>
select new {t.Title, t.FileLocation};<br>
<br>
</code></pre><br>

Then we�ll just iterate over the results and wait for a keypress before quitting.<br>
<br>
<br><pre><code><br>
foreach(var track in q){<br>
Console.WriteLine(track.Title + ": " + track.FileLocation);<br>
}<br>
Console.ReadKey();<br>
<br>
</code></pre><br>

That�s all there is to it. Of course there�s a lot more going on behind the<br>
scenes, but the beauty of LINQ is that you don�t need to see all of that while<br>
you�re only interested in getting some =Track= s back! In the references section<br>
I�ll give some links that you can go to if you want to know what�s going on<br>
under the hood.<br>
<br>
<h2>Using the graphical designer to design an ontology</h2>
The process described in previous sections can be done easily using the<br>
=LinqToRdfDesigner= . This section describes the tasks required to set up a very<br>
simple ontology of two classes called =Artifact=  and =Assembly= . The program<br>
associated with it is available from the google code site via subversion. It�s<br>
purpose is to gather various bits of information about the running system and<br>
store them as objects in an N3 file for later use. We won�t explore too much of<br>
the application except where it depends on LinqToRdf or LinqToRdfDesigner.<br>
<br>
The first task is to create a design surface to draw the domain model on. The<br>
file extension for LinqToRdfDesigner files is �=rdfx= �. Click on the<br>
�\textsf{New Item}�<br>
button on the standard toolbar.<br>
<br>
\begin{figure}<br>
\centering<br>
\includegraphics{images/img3}<br>
\caption{The New Items button.}<br>
\end{figure}<br>
<br>
The item templates selection dialog will then appear. You will locate the<br>
LinqToRdf item template at the bottom, under \textsf{My Templates}. If it is not there,<br>
then check that the LinqToRdfDesigner has been installed on your machine.<br>
<br>
\begin{figure}<br>
\centering<br>
\includegraphics{images/img4}<br>
\caption{The file templates selection dialog showing the LinqToRdf designer template.}<br>
\end{figure}<br>
<br>
Change the name of the file to =SystemScannerModel.rdfx= . This will create<br>
several files in your project.<br>
<br>
\begin{figure}<br>
\centering<br>
\includegraphics{images/img5}<br>
\caption{The resulting files using the name that you provide.}<br>
\end{figure}<br>
<br>
=SystemScannerModel.rdfx=  is the design surface file.<br>
=SystemScannerModel.rdfx.entities.tt=  is a text template file that is used to<br>
generate the C# file containing the base definitions of your .NET entity model.<br>
=SystemScannerModel.rdfx.n3.tt=  is the text template that is used to generate the<br>
N3 format ontology definition that will be used by LinqToRdf and its<br>
components.<br>
<br>
Double click on the rdfx file to open the design surface. You should now notice<br>
that the toolbox contains elements for modeling your classes.<br>
<br>
\begin{figure}<br>
\centering<br>
\includegraphics{images/img6}<br>
\caption{The toolbox when you're editing a LinqToRdf design.}<br>
\end{figure}<br>
<br>
Drag a class from the toolbox onto the design surface.<br>
<br>
\begin{figure}<br>
\centering<br>
\includegraphics{images/img7}<br>
\caption{The default appearance of a class when first dragged onto a design surface.}<br>
\end{figure}<br>
<br>
By default, it will be called something like =ModelClass1= . Rename it to<br>
�=Artifact= �.<br>
This class also needs a name within the ontology. In semantic web applications,<br>
classes are named using URIs. You will need to choose a URI for the class.<br>
We�ll get onto that in a moment. For now, press the F4 button an enter<br>
�=Artifact= � in the �\textsf{Owl Class Uri}� property in the property window.<br>
<br>
Your properties window should now look like this.<br>
<br>
\begin{figure}<br>
\centering<br>
\includegraphics{images/img8}<br>
\caption{Editing the properties of a class on the design surface.}<br>
\end{figure}<br>
<br>
Right click on the class shape in the designer and click on the<br>
�\textsf{Add New Model<br>
Attribute}� menu option.<br>
<br>
\begin{figure}<br>
\centering<br>
\includegraphics{images/img9}<br>
\caption{Adding a new model attribute.}<br>
\end{figure}<br>
<br>
Add an attribute (a field in .NET parlance) to the class and call it<br>
�=ArtifactExists= �. Again, use the properties window to set up some properties<br>
for the attribute. Initially it will look like this.<br>
<br>
\begin{figure}<br>
\centering<br>
\includegraphics{images/img10}<br>
\caption{The default properties of a class property.}<br>
\end{figure}<br>
<br>
Change the properties to be like this.<br>
<br>
\begin{figure}<br>
\centering<br>
\includegraphics{images/img11}<br>
\caption{A properly specified attribute has a reference to the property URI<br>
that matches the object or data property in the ontology.}<br>
\end{figure}<br>
<br>
What we�re saying here is that the .NET property �=ArtifactExists= � of .NET type<br>
=bool=  corresponds to the OWL URI �=artifactExists= � which LinqToRdf will convert<br>
into the XML Schema Datatype �=xsdt:boolean= �. You can ignore the description<br>
property, for the moment, since it�s not currently used in the designer text<br>
templates. Later on we will see how these extra properties can be used within<br>
the text templates to generate documentation for both C# and N3.<br>
<br>
The rest of the attributes attached to the artifact class are shown below.<br>
<br>
\begin{figure}<br>
\centering<br>
\includegraphics{images/img12}<br>
\caption{The class with all of the properties defined.}<br>
\end{figure}<br>
<br>
Their types should be easy enough to guess, and the OWL URIs are just the same,<br>
except using camel rather than Pascal case, since that is the norm in N3.<br>
<br>
Next drag another class from the toolbox onto the design surface and call it<br>
`=Assembly= '.<br>
<br>
\begin{figure}<br>
\centering<br>
\includegraphics{images/img13}<br>
\caption{Two classes related by an inheritance relationship.}<br>
\end{figure}<br>
<br>
Use the �\textsf{inheritance}� tool from the toolbox to connect from it to the class<br>
�Artifact�. This will connect the classes using direct inheritance in C# and<br>
=owl:subclass=  in N3. Your domain model is now taking shape.<br>
<br>
You�re now at a stage where you have something worth converting into code!<br>
Click on the �\textsf{transform all templates}� button on the Solution Explorer toolbar.<br>
<br>
\begin{figure}<br>
\centering<br>
\includegraphics{images/img14}<br>
\caption{Invoking the code generator, using the 'transform all templates' button.}<br>
\end{figure}<br>
<br>
This processes the text templates that were created earlier, supplying them<br>
with the object model that you�ve built up over the last few steps. The output<br>
should look like this.<br>
<br>
\begin{figure}<br>
\centering<br>
\includegraphics{images/img16}<br>
\caption{The output from running the code generator.}<br>
\end{figure}<br>
<br>
If you look inside the code generated for the entities, it should look like this.<br>
<br>
<br><pre><code><br>
namespace SystemScannerModel<br>
{<br>
<br>
[OwlResource(OntologyName = "&lt;math&gt;fileinputname<br>
<br>
Unknown end tag for &lt;/math&gt;<br>
<br>
",<br>
RelativeUriReference = "Artifact")]<br>
public partial class Artifact : OwlInstanceSupertype<br>
{<br>
[OwlResource(OntologyName = "SystemScannerModel ",<br>
RelativeUriReference = "artifactExists")]<br>
public bool ArtifactExists { get; set; }<br>
[OwlResource(OntologyName = "SystemScannerModel ",<br>
RelativeUriReference = "dateCreated")]<br>
public DateTime DateCreated { get; set; }<br>
[OwlResource(OntologyName = "SystemScannerModel ",<br>
RelativeUriReference = "dateLastModified")]<br>
public DateTime DateLastModified { get; set; }<br>
[OwlResource(OntologyName = "SystemScannerModel ",<br>
RelativeUriReference = "filePath")]<br>
public string FilePath { get; set; }<br>
[OwlResource(OntologyName = "SystemScannerModel ",<br>
RelativeUriReference = "isReadOnly")]<br>
public bool IsReadOnly { get; set; }<br>
}<br>
<br>
[OwlResource(OntologyName = "&lt;math&gt;fileinputname<br>
<br>
Unknown end tag for &lt;/math&gt;<br>
<br>
",<br>
RelativeUriReference = "assembly")]<br>
public partial class Assembly : Artifact<br>
{<br>
[OwlResource(OntologyName = "SystemScannerModel ",<br>
RelativeUriReference = "isSigned")]<br>
public bool IsSigned { get; set; }<br>
[OwlResource(OntologyName = "SystemScannerModel ",<br>
RelativeUriReference = "strongKey")]<br>
public string StrongKey { get; set; }<br>
[OwlResource(OntologyName = "SystemScannerModel ",<br>
RelativeUriReference = "version")]<br>
public string Version { get; set; }<br>
}<br>
}<br>
<br>
</code></pre><br>

Here is an example of a unit test that grabs the data from the assembly.<br>
=ArtifactStore=  is a wrapper around a SemWeb =MemoryStore= .<br>
<br>
<br><pre><code><br>
string loc = @"C:\\etc\\dev\\prototypes\\linqtordf\\"<br>
+"SystemScanner\\rdf\\sys.artifacts.n3";<br>
ArtifactStore store = new ArtifactStore(loc);<br>
Dictionary&lt;string, AssemblyName&gt; tmpStore =<br>
new Dictionary&lt;string, AssemblyName&gt;();<br>
Extensions.Scan(GetType().Assembly.GetName(), tmpStore);<br>
foreach (AssemblyName asmName in tmpStore.Values)<br>
{<br>
store.Add(new SystemScannerModel.Assembly(asmName.GetAssembly()));<br>
}<br>
Assert.AreEqual(344, store.TripleStore.StatementCount);<br>
<br>
</code></pre><br>

Before this code will build you need to add a few assembly references to<br>
LinqToRdf, SemWeb and LINQ to your project.<br>
<br>
\begin{figure}<br>
\centering<br>
\includegraphics{images/img15}<br>
\caption{Necessary assembly references for working with LinqToRdf.}<br>
\end{figure}<br>
<br>
Now rebuild your solution. Assuming there are no other compile time errors in<br>
the system it should build OK. You are now able to perform LinqToRdf queries<br>
against the ontology using the techniques described previously.<br>
<br>
<br>
<h1>Navigating relationships in LinqToRdf</h1>
The representation and querying of relationships in RDF is very different from<br>
the model employed in relational databases. At first glance, looking at LINQ to<br>
SQL, you might think that that LINQ itself was tied to the relational model,<br>
but that is not the case. LINQ is a fully extensible platform allowing us to represent<br>
other kinds of relationship through the use of query operators.<br>
<br>
In LinqToRdf you can use the =StmtObjectWithSubjectAndPredicate=  and<br>
=StmtSubjectWithObjectAndPredicate=  to<br>
instruct it to navigate the relationships using SPARQL syntax. here is an<br>
example of =StmtObjectWithSubjectAndPredicate=  in action:<br>
<br>
<br><pre><code><br>
var ctx = new MusicDataContext(@"http://localhost/linqtordf/SparqlQuery.aspx");<br>
var album = (from a in ctx.Albums<br>
where a.Name.StartsWith("Thomas")<br>
select a).First();<br>
<br>
string recordUri = album.InstanceUri;<br>
string trackPredicateUri = album.PredicateUriForProperty(MethodBase.GetCurrentMethod());<br>
var tracks = from t in ((MusicDataContext)DataContext).Tracks<br>
where t.StmtSubjectWithObjectAndPredicate(recordUri, trackPredicateUri)<br>
select t;<br>
</code></pre><br>

Given an object called =album=  whose instance URI we already know, we can<br>
select through properties that we know reference that URI. The definition of<br>
property =Album=  in class =Track=  looks like this:<br>
<br>
<br><pre><code><br>
[OwlResource(OntologyName = "Music", RelativeUriReference = "isTrackOn")]<br>
public Album Album<br>
{<br>
get<br>
{<br>
//...<br>
<br>
</code></pre><br>

The combination of =t= , =Album=  and =StmtSubjectWithObjectAndPredicate=  is enough<br>
for LinqToRdf to be able to generate SPARQL with a triple like this:<br>
<br>
<br><pre><code><br>
&lt;math&gt;t ns1:isTrackOn ns1:Album_2 .<br>
</code></pre><br>

Which will filter all tracks except those that are tracks on =ns1:Album_2= .<br>
To support this, LinqToRdf automatically retrieves and stores the instance URIs<br>
for all ontology classes. If you use projections, then you don't get the<br>
=InstanceUri=  property, so you will have to construct your collections<br>
manually.<br>
<br>
LinqToRdf also stores a reference to the DataContext from an instance was<br>
retrieved with the instance itself. An instance can therefore define parent<br>
child collections like this:<br>
<br>
<br><pre><code><br>
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
<br>
string recordUri = this.InstanceUri;<br>
string trackPredicateUri =<br>
this.PredicateUriForProperty(MethodBase.GetCurrentMethod());<br>
_Tracks.SetSource(<br>
from t in ((MusicDataContext)DataContext).Tracks<br>
where t.StmtSubjectWithObjectAndPredicate(recordUri,<br>
trackPredicateUri)<br>
select t);<br>
}<br>
return _Tracks;<br>
}<br>
}<br>
</code></pre><br>

This uses the EntitySet container that can be primed with a query based on the<br>
=DataContext=  and =InstanceUri=  of =this= . It will store the<br>
query and only invoke it on first demand. You need to beware that the<br>
=DataContext=  has not gone out of scope before the query gets invoked. That<br>
constraint is in common with LINQ to SQL.<br>
<br>
<br><pre><code><br>
using System.Data.Linq;<br>
using System.Linq;<br>
using System.Reflection;<br>
using ID3Lib;<br>
using LinqToRdf;<br>
<br>
namespace RdfMusic<br>
{<br>
public class MusicDataContext : RdfDataContext<br>
{<br>
public MusicDataContext(TripleStore store) :<br>
base(store){}<br>
<br>
public MusicDataContext(string store) :<br>
base(new TripleStore(store)) { }<br>
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
}<br>
<br>
[OwlResource(OntologyName = "Music",<br>
RelativeUriReference = "Track")]<br>
public class Track : OwlInstanceSupertype<br>
{<br>
public Track(TagHandler th, string fileLocation)<br>
{<br>
FileLocation = fileLocation;<br>
Title = th.Track;<br>
ArtistName = th.Artist;<br>
AlbumName = th.Album;<br>
Year = th.Year;<br>
GenreName = th.Genere;<br>
Comment = th.Comment;<br>
}<br>
<br>
public Track()<br>
{<br>
}<br>
<br>
[OwlResource(OntologyName = "Music",<br>
RelativeUriReference = "title")]<br>
public string Title { get; set; }<br>
<br>
[OwlResource(OntologyName = "Music",<br>
RelativeUriReference = "artistName")]<br>
public string ArtistName { get; set; }<br>
<br>
[OwlResource(OntologyName = "Music",<br>
RelativeUriReference = "albumName")]<br>
public string AlbumName { get; set; }<br>
<br>
[OwlResource(OntologyName = "Music",<br>
RelativeUriReference = "year")]<br>
public string Year { get; set; }<br>
<br>
[OwlResource(OntologyName = "Music",<br>
RelativeUriReference = "genreName")]<br>
public string GenreName { get; set; }<br>
<br>
[OwlResource(OntologyName = "Music",<br>
RelativeUriReference = "comment")]<br>
public string Comment { get; set; }<br>
<br>
[OwlResource(OntologyName = "Music",<br>
RelativeUriReference = "fileLocation")]<br>
public string FileLocation { get; set; }<br>
<br>
[OwlResource(OntologyName = "Music",<br>
RelativeUriReference = "rating")]<br>
public int Rating { get; set; }<br>
<br>
private EntityRef&lt;Album&gt; _Album { get; set; }<br>
<br>
[OwlResource(OntologyName = "Music",<br>
RelativeUriReference = "isTrackOn")]<br>
public Album Album<br>
{<br>
get<br>
{<br>
if (_Album.HasLoadedOrAssignedValue)<br>
return _Album.Entity;<br>
if (DataContext != null)<br>
{<br>
var ctx = (MusicDataContext) DataContext;<br>
string trackUri = InstanceUri;<br>
string trackPredicateUri =<br>
this.PredicateUriForProperty(MethodBase.GetCurrentMethod());<br>
_Album = new EntityRef&lt;Album&gt;(<br>
from r in ((MusicDataContext) DataContext).Albums<br>
where r.StmtObjectWithSubjectAndPredicate(trackUri,<br>
trackPredicateUri)<br>
select r);<br>
<br>
return _Album.Entity;<br>
}<br>
return null;<br>
}<br>
}<br>
}<br>
<br>
[OwlResource(OntologyName = "Music",<br>
RelativeUriReference = "Album")]<br>
public class Album : OwlInstanceSupertype<br>
{<br>
private readonly EntitySet&lt;Track&gt; _Tracks =<br>
new EntitySet&lt;Track&gt;();<br>
<br>
[OwlResource(OntologyName = "Music",<br>
RelativeUriReference = "name")]<br>
public string Name { get; set; }<br>
<br>
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
string recordUri = InstanceUri;<br>
string trackPredicateUri =<br>
this.PredicateUriForProperty(MethodBase.GetCurrentMethod());<br>
_Tracks.SetSource(<br>
from t in ((MusicDataContext) DataContext).Tracks<br>
where t.StmtSubjectWithObjectAndPredicate(recordUri,<br>
trackPredicateUri)<br>
select t);<br>
}<br>
return _Tracks;<br>
}<br>
}<br>
}<br>
}<br>
</code></pre><br>

<br><pre><code><br>
public static void Main()<br>
{<br>
var ctx = new MusicDataContext(<br>
@"http://localhost/linqtordf/SparqlQuery.aspx");<br>
var track = (from t in ctx.Tracks<br>
where t.StmtObjectWithSubjectAndPredicate(<br>
"http://aabs.purl.org/ontologies/2007/04/music#Track_-861912094",<br>
"http://aabs.purl.org/ontologies/2007/04/music#isTrackOn")<br>
select t).First();<br>
<br>
Debug.WriteLine("Track was " + track.Title);<br>
Debug.WriteLine("Album was " + track.Album.Name);<br>
<br>
foreach (var t in track.Album.Tracks)<br>
{<br>
Debug.WriteLine("Album Track was " + track.Title);<br>
}<br>
}<br>
</code></pre><br>