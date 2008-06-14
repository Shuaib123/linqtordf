using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LinqToRdf;
using System.Data.Linq;

namespace Some.Namespace
{
[assembly: Ontology(
    BaseUri = "http://xmlns.com/foaf/0.1/",
    Name = "MyOntology",
    Prefix = "MyOntology",
    UrlOfOntology = "http://xmlns.com/foaf/0.1/")]


    public partial class MyOntologyDataContext : RdfDataContext
    {
        public MyOntologyDataContext(TripleStore store) : base(store)
        {
        }
        public MyOntologyDataContext(string store) : base(new TripleStore(store))
        {
        }

		        public IQueryable<Person> Persons
		        {
		            get
		            {
		                return ForType<Person>();
		            }
		        }
		
		        public IQueryable<Document> Documents
		        {
		            get
		            {
		                return ForType<Document>();
		            }
		        }
		
		        public IQueryable<Agent> Agents
		        {
		            get
		            {
		                return ForType<Agent>();
		            }
		        }
		
		        public IQueryable<Organization> Organizations
		        {
		            get
		            {
		                return ForType<Organization>();
		            }
		        }
		
		        public IQueryable<Project> Projects
		        {
		            get
		            {
		                return ForType<Project>();
		            }
		        }
		
		        public IQueryable<Group> Groups
		        {
		            get
		            {
		                return ForType<Group>();
		            }
		        }
		
		        public IQueryable<Image> Images
		        {
		            get
		            {
		                return ForType<Image>();
		            }
		        }
		
		        public IQueryable<PersonalProfileDocument> PersonalProfileDocuments
		        {
		            get
		            {
		                return ForType<PersonalProfileDocument>();
		            }
		        }
		
		        public IQueryable<OnlineAccount> OnlineAccounts
		        {
		            get
		            {
		                return ForType<OnlineAccount>();
		            }
		        }
		
		        public IQueryable<OnlineGamingAccount> OnlineGamingAccounts
		        {
		            get
		            {
		                return ForType<OnlineGamingAccount>();
		            }
		        }
		
		        public IQueryable<OnlineEcommerceAccount> OnlineEcommerceAccounts
		        {
		            get
		            {
		                return ForType<OnlineEcommerceAccount>();
		            }
		        }
		
		        public IQueryable<OnlineChatAccount> OnlineChatAccounts
		        {
		            get
		            {
		                return ForType<OnlineChatAccount>();
		            }
		        }
		

    }

[OwlResource(OntologyName="MyOntology", RelativeUriReference="Person")]
public partial class Person : OwlInstanceSupertype
{
#region Datatype properties
  [OwlResource(OntologyName = "MyOntology", RelativeUriReference = "surname")]
  public string surname {get;set;} // Person
  [OwlResource(OntologyName = "MyOntology", RelativeUriReference = "family_name")]
  public string family_name {get;set;} // Person
  [OwlResource(OntologyName = "MyOntology", RelativeUriReference = "geekcode")]
  public string geekcode {get;set;} // Person
  [OwlResource(OntologyName = "MyOntology", RelativeUriReference = "firstName")]
  public string firstName {get;set;} // Person
  [OwlResource(OntologyName = "MyOntology", RelativeUriReference = "plan")]
  public string plan {get;set;} // Person
  [OwlResource(OntologyName = "MyOntology", RelativeUriReference = "myersBriggs")]
  public string myersBriggs {get;set;} // Person
  [OwlResource(OntologyName = "MyOntology", RelativeUriReference = "workInfoHomepage")]
  public Document workInfoHomepage {get;set;} // Person
  [OwlResource(OntologyName = "MyOntology", RelativeUriReference = "schoolHomepage")]
  public Document schoolHomepage {get;set;} // Person
  [OwlResource(OntologyName = "MyOntology", RelativeUriReference = "interest")]
  public Document interest {get;set;} // Person
  [OwlResource(OntologyName = "MyOntology", RelativeUriReference = "topic_interest")]
  public LinqToRdf.OwlInstanceSupertype topic_interest {get;set;} // Person
  [OwlResource(OntologyName = "MyOntology", RelativeUriReference = "publications")]
  public Document publications {get;set;} // Person
  [OwlResource(OntologyName = "MyOntology", RelativeUriReference = "currentProject")]
  public LinqToRdf.OwlInstanceSupertype currentProject {get;set;} // Person
  [OwlResource(OntologyName = "MyOntology", RelativeUriReference = "pastProject")]
  public LinqToRdf.OwlInstanceSupertype pastProject {get;set;} // Person

#endregion

#region Incoming relationships properties
private EntitySet<Person> _knowss = new EntitySet<Person>();
[OwlResource(OntologyName = "MyOntology", RelativeUriReference = "knows")]
public EntitySet<Person> knows
{
    get
    {
        if (_knowss.HasLoadedOrAssignedValues)
            return _knowss;
        if (DataContext != null)
        {
            var ctx = (MyOntologyDataContext)DataContext;
            _knowss.SetSource(from t in ctx.Persons
								  where t.knows.HavingSubjectUri(this.InstanceUri)
								  select t);
        }
        return _knowss;
    }
}

#endregion

#region Object properties
[OwlResource(OntologyName = "MyOntology", RelativeUriReference = "http://xmlns.com/foaf/0.1/knows")]
public string PersonUri { get; set; }

private EntityRef<Person> _Person { get; set; }

[OwlResource(OntologyName = "MyOntology", RelativeUriReference = "http://xmlns.com/foaf/0.1/knows")]
public Person Person
{
    get
    {
        if (_Person.HasLoadedOrAssignedValue)
            return _Person.Entity;
        if (DataContext != null)
        {
            var ctx = (MyOntologyDataContext)DataContext;
            _Person = new EntityRef<Person>(from x in ctx.Persons where x.HasInstanceUri(PersonUri) select x);
            return _Person.Entity;
        }
        return null;
    }
}
[OwlResource(OntologyName = "MyOntology", RelativeUriReference = "http://xmlns.com/foaf/0.1/img")]
public string ImageUri { get; set; }

private EntityRef<Image> _Image { get; set; }

[OwlResource(OntologyName = "MyOntology", RelativeUriReference = "http://xmlns.com/foaf/0.1/img")]
public Image Image
{
    get
    {
        if (_Image.HasLoadedOrAssignedValue)
            return _Image.Entity;
        if (DataContext != null)
        {
            var ctx = (MyOntologyDataContext)DataContext;
            _Image = new EntityRef<Image>(from x in ctx.Images where x.HasInstanceUri(ImageUri) select x);
            return _Image.Entity;
        }
        return null;
    }
}
[OwlResource(OntologyName = "MyOntology", RelativeUriReference = "http://xmlns.com/foaf/0.1/workplaceHomepage")]
public string DocumentUri { get; set; }

private EntityRef<Document> _Document { get; set; }

[OwlResource(OntologyName = "MyOntology", RelativeUriReference = "http://xmlns.com/foaf/0.1/workplaceHomepage")]
public Document Document
{
    get
    {
        if (_Document.HasLoadedOrAssignedValue)
            return _Document.Entity;
        if (DataContext != null)
        {
            var ctx = (MyOntologyDataContext)DataContext;
            _Document = new EntityRef<Document>(from x in ctx.Documents where x.HasInstanceUri(DocumentUri) select x);
            return _Document.Entity;
        }
        return null;
    }
}

#endregion
}

[OwlResource(OntologyName="MyOntology", RelativeUriReference="Document")]
public partial class Document : OwlInstanceSupertype
{
#region Datatype properties
  [OwlResource(OntologyName = "MyOntology", RelativeUriReference = "primaryTopic")]
  public LinqToRdf.OwlInstanceSupertype primaryTopic {get;set;} // Document
  [OwlResource(OntologyName = "MyOntology", RelativeUriReference = "topic")]
  public LinqToRdf.OwlInstanceSupertype topic {get;set;} // Document

#endregion

#region Incoming relationships properties
private EntitySet<Person> _workplaceHomepages = new EntitySet<Person>();
[OwlResource(OntologyName = "MyOntology", RelativeUriReference = "workplaceHomepage")]
public EntitySet<Person> workplaceHomepage
{
    get
    {
        if (_workplaceHomepages.HasLoadedOrAssignedValues)
            return _workplaceHomepages;
        if (DataContext != null)
        {
            var ctx = (MyOntologyDataContext)DataContext;
            _workplaceHomepages.SetSource(from t in ctx.Persons
								  where t.workplaceHomepage.HavingSubjectUri(this.InstanceUri)
								  select t);
        }
        return _workplaceHomepages;
    }
}

#endregion

#region Object properties
#endregion
}

[OwlResource(OntologyName="MyOntology", RelativeUriReference="Agent")]
public partial class Agent : OwlInstanceSupertype
{
#region Datatype properties
  [OwlResource(OntologyName = "MyOntology", RelativeUriReference = "mbox_sha1sum")]
  public string mbox_sha1sum {get;set;} // Agent
  [OwlResource(OntologyName = "MyOntology", RelativeUriReference = "gender")]
  public string gender {get;set;} // Agent
  [OwlResource(OntologyName = "MyOntology", RelativeUriReference = "jabberID")]
  public string jabberID {get;set;} // Agent
  [OwlResource(OntologyName = "MyOntology", RelativeUriReference = "aimChatID")]
  public string aimChatID {get;set;} // Agent
  [OwlResource(OntologyName = "MyOntology", RelativeUriReference = "icqChatID")]
  public string icqChatID {get;set;} // Agent
  [OwlResource(OntologyName = "MyOntology", RelativeUriReference = "yahooChatID")]
  public string yahooChatID {get;set;} // Agent
  [OwlResource(OntologyName = "MyOntology", RelativeUriReference = "msnChatID")]
  public string msnChatID {get;set;} // Agent
  [OwlResource(OntologyName = "MyOntology", RelativeUriReference = "birthday")]
  public string birthday {get;set;} // Agent
  [OwlResource(OntologyName = "MyOntology", RelativeUriReference = "mbox")]
  public LinqToRdf.OwlInstanceSupertype mbox {get;set;} // Agent
  [OwlResource(OntologyName = "MyOntology", RelativeUriReference = "weblog")]
  public Document weblog {get;set;} // Agent
  [OwlResource(OntologyName = "MyOntology", RelativeUriReference = "openid")]
  public Document openid {get;set;} // Agent
  [OwlResource(OntologyName = "MyOntology", RelativeUriReference = "tipjar")]
  public Document tipjar {get;set;} // Agent
  [OwlResource(OntologyName = "MyOntology", RelativeUriReference = "made")]
  public LinqToRdf.OwlInstanceSupertype made {get;set;} // Agent
  [OwlResource(OntologyName = "MyOntology", RelativeUriReference = "holdsAccount")]
  public OnlineAccount holdsAccount {get;set;} // Agent

#endregion

#region Incoming relationships properties
#endregion

#region Object properties
#endregion
}

[OwlResource(OntologyName="MyOntology", RelativeUriReference="Organization")]
public partial class Organization : OwlInstanceSupertype
{
#region Datatype properties
#endregion

#region Incoming relationships properties
#endregion

#region Object properties
#endregion
}

[OwlResource(OntologyName="MyOntology", RelativeUriReference="Project")]
public partial class Project : OwlInstanceSupertype
{
#region Datatype properties
#endregion

#region Incoming relationships properties
#endregion

#region Object properties
#endregion
}

[OwlResource(OntologyName="MyOntology", RelativeUriReference="Group")]
public partial class Group : OwlInstanceSupertype
{
#region Datatype properties
  [OwlResource(OntologyName = "MyOntology", RelativeUriReference = "member")]
  public Agent member {get;set;} // Group

#endregion

#region Incoming relationships properties
#endregion

#region Object properties
#endregion
}

[OwlResource(OntologyName="MyOntology", RelativeUriReference="Image")]
public partial class Image : OwlInstanceSupertype
{
#region Datatype properties
  [OwlResource(OntologyName = "MyOntology", RelativeUriReference = "depicts")]
  public LinqToRdf.OwlInstanceSupertype depicts {get;set;} // Image
  [OwlResource(OntologyName = "MyOntology", RelativeUriReference = "thumbnail")]
  public Image thumbnail {get;set;} // Image

#endregion

#region Incoming relationships properties
private EntitySet<Person> _imgs = new EntitySet<Person>();
[OwlResource(OntologyName = "MyOntology", RelativeUriReference = "img")]
public EntitySet<Person> img
{
    get
    {
        if (_imgs.HasLoadedOrAssignedValues)
            return _imgs;
        if (DataContext != null)
        {
            var ctx = (MyOntologyDataContext)DataContext;
            _imgs.SetSource(from t in ctx.Persons
								  where t.img.HavingSubjectUri(this.InstanceUri)
								  select t);
        }
        return _imgs;
    }
}

#endregion

#region Object properties
#endregion
}

[OwlResource(OntologyName="MyOntology", RelativeUriReference="PersonalProfileDocument")]
public partial class PersonalProfileDocument : OwlInstanceSupertype
{
#region Datatype properties
#endregion

#region Incoming relationships properties
#endregion

#region Object properties
#endregion
}

[OwlResource(OntologyName="MyOntology", RelativeUriReference="OnlineAccount")]
public partial class OnlineAccount : OwlInstanceSupertype
{
#region Datatype properties
  [OwlResource(OntologyName = "MyOntology", RelativeUriReference = "accountName")]
  public string accountName {get;set;} // OnlineAccount
  [OwlResource(OntologyName = "MyOntology", RelativeUriReference = "accountServiceHomepage")]
  public Document accountServiceHomepage {get;set;} // OnlineAccount

#endregion

#region Incoming relationships properties
#endregion

#region Object properties
#endregion
}

[OwlResource(OntologyName="MyOntology", RelativeUriReference="OnlineGamingAccount")]
public partial class OnlineGamingAccount : OwlInstanceSupertype
{
#region Datatype properties
#endregion

#region Incoming relationships properties
#endregion

#region Object properties
#endregion
}

[OwlResource(OntologyName="MyOntology", RelativeUriReference="OnlineEcommerceAccount")]
public partial class OnlineEcommerceAccount : OwlInstanceSupertype
{
#region Datatype properties
#endregion

#region Incoming relationships properties
#endregion

#region Object properties
#endregion
}

[OwlResource(OntologyName="MyOntology", RelativeUriReference="OnlineChatAccount")]
public partial class OnlineChatAccount : OwlInstanceSupertype
{
#region Datatype properties
#endregion

#region Incoming relationships properties
#endregion

#region Object properties
#endregion
}



}