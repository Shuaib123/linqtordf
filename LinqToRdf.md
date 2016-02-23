# Introduction #

This page provides a set of usage examples showing you how to work with LinqToRdf

# Details #

```
MusicDataContext ctx = new MusicDataContext(@"http://localhost/linqtordf/SparqlQuery.aspx");
var q = (from t in ctx.Tracks
         where t.Year == "2006" &&
               t.GenreName == "History 5 | Fall 2006 | UC Berkeley"
         orderby t.FileLocation
         select new {t.Title, t.FileLocation}).Skip(10).Take(5);

foreach (var track in q)
{
   Console.WriteLine(track.Title + ": " + track.FileLocation);
} 
```