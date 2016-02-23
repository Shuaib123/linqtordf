# Introduction #

This page lists some very high level ideas about how LinqToRdf could develop.

## v0.9 ##
  * conversion of build systems over to NANT
  * extensive use of logging
  * use of INotifyPropertyChanged on all properties of the generated objects
  * completion of RdfMetal feature set
  * conversion of manuals etc over to wiki format as part of build process
  * host manual on the Wiki
  * use of TPL to improve performance of query creation process
  * use of TPL to improve performance of the object deserialiser

## v0.10 ##
  * provide merging of semweb assemblies using ILMerge in NANT
  * include latest version (v1.061+) of semweb libraries
  * find a way to deploy RdfMetal templates as embedded resources
  * perf counters
  * convergence on a single test data set
  * SQL Server hosting support for all test data
  * compilation of manuals from LaTeX source as part of build process

## v1.0 ##
  * proven reasoner support
  * mono compatibility
  * 100% code coverage
  * full documented code
  * NDoc code export as part of the release cycle
  * SPARUL support
  * certified compliance with all SPARQL operators
  * full load tests

## v2.0 ##
  * RDF metamodel through reflection
  * expose DB info as RDF
    * general purpose mapping framework (i.e. what I suggested to RDB2RDF team)
