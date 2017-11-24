# Trias.DataService
A .NET client proxy for a web services implementing Trias interface for open public transport services.
For example Germans's regional transport company VRN uses it - https://www.vrn.de/service/entwickler/openservice/index.html
The protocol is devloped and standardized by VDV (The Association of a German Transport Companies) as part of IP-KOM-ÖV project

Trias.DataService nuget package contains data model classes generated out provided XSD schema files (there are couple of supported version) and client class to communicate with the service.
For data model classes generation I used free tool called Xsd2Code (https://xsd2code.codeplex.com/)
