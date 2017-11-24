# Trias.DataService

[![Build status](https://ci.appveyor.com/api/projects/status/8ba2g5yglvwcgib1?svg=true)](https://ci.appveyor.com/project/limanartem/trias-dataservice)


A .NET client proxy for a web services implementing Trias interface for open public transport services.
For example Germans's regional transport company VRN uses it - https://www.vrn.de/service/entwickler/openservice/index.html

The protocol is devloped and standardized by VDV (The Association of a German Transport Companies) as part of IP-KOM-ÖV project

Trias.DataService nuget package contains data model classes generated out provided XSD schema files (there are couple of supported version) and client class to communicate with the service.

For data model classes generation I used free tool called Xsd2Code (https://xsd2code.codeplex.com/)

````csharp

var client = new TriasServiceClient("<service url>", "<ref>");

var input = new LocationInformationRequestStructure()
{
	Item = new InitialLocationInputStructure()
	{
		GeoRestriction = new GeoRestrictionsStructure()
		{
			Item = new GeoCircleStructure()
			{
				Center = new GeoPositionStructure()
				{
					Longitude = (decimal) 8.687699,
					Latitude = (decimal) 49.427390
				},
				Radius = "100"
			}
		}
	},
	Restrictions = new LocationParamStructure()
	{
		Type = new[]
		{
			LocationTypeEnumeration.stop
		}
	}
};

var result = await client.Request(input);


```
