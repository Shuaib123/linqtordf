 
public partial class Person
    {
    	Literal surname {get;set;}
    	Literal family_name {get;set;}
    	Literal geekcode {get;set;}
    	Literal firstName {get;set;}
    	Literal plan {get;set;}
    	Person knows {get;set;}
    	Image img {get;set;}
    	Literal myersBriggs {get;set;}
    	Document workplaceHomepage {get;set;}
    	Document workInfoHomepage {get;set;}
    	Document schoolHomepage {get;set;}
    	Document interest {get;set;}
    	Thing topic_interest {get;set;}
    	Document publications {get;set;}
    	Thing currentProject {get;set;}
    	Thing pastProject {get;set;}
        }
    public partial class Document
    {
    	Thing primaryTopic {get;set;}
    	Thing topic {get;set;}
        }
    public partial class Agent
    {
    	Literal mbox_sha1sum {get;set;}
    	Literal gender {get;set;}
    	Literal jabberID {get;set;}
    	Literal aimChatID {get;set;}
    	Literal icqChatID {get;set;}
    	Literal yahooChatID {get;set;}
    	Literal msnChatID {get;set;}
    	Literal birthday {get;set;}
    	Thing mbox {get;set;}
    	Document weblog {get;set;}
    	Document openid {get;set;}
    	Document tipjar {get;set;}
    	Thing made {get;set;}
    	OnlineAccount holdsAccount {get;set;}
        }
    public partial class Organization
    {
        }
    public partial class Project
    {
        }
    public partial class Group
    {
    	Agent member {get;set;}
        }
    public partial class Image
    {
    	Thing depicts {get;set;}
    	Image thumbnail {get;set;}
        }
    public partial class PersonalProfileDocument
    {
        }
    public partial class OnlineAccount
    {
    	Literal accountName {get;set;}
    	Document accountServiceHomepage {get;set;}
        }
    public partial class OnlineGamingAccount
    {
        }
    public partial class OnlineEcommerceAccount
    {
        }
    public partial class OnlineChatAccount
    {
        }
    