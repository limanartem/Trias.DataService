using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Trias.DataService.Tests.Helpers;
using Trias.DataService.v1_0;
using Trias.DataService.v1_0.DataModel;

namespace Trias.DataService.Tests
{
    [TestFixture]
    public class LocationInformationRequestTests
    {
        [Test]
        public async Task Request_Test()
        {
            //Arrange
            var client = new TriasServiceClient(ConfigHelper.TriasServiceUrl, ConfigHelper.TriasServiceRef);

            var input = new LocationInformationRequestStructure
            {
                Item = new InitialLocationInputStructure
                {
                    GeoRestriction = new GeoRestrictionsStructure
                    {
                        Item = new GeoCircleStructure
                        {
                            Center = new GeoPositionStructure
                            {
                                Longitude = (decimal) 8.687699,
                                Latitude = (decimal) 49.427390
                            },
                            Radius = "100"
                        }
                    }
                },
                Restrictions = new LocationParamStructure
                {
                    Type = new[]
                    {
                        LocationTypeEnumeration.stop
                    }
                }
            };

            //Act
            var result = await client.Request(input);

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Location, Is.Not.Null);
            Assert.That(result.Location.Length, Is.GreaterThan(0));
            Assert.IsInstanceOf<StopPointStructure>(result.Location[0].Location.Item);
            Assert.IsTrue(result.Location.Any(l =>
                ((StopPointStructure) l.Location.Item).StopPointRef.Value == "de:08221:6827"));
        }
    }
}