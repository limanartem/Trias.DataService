using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Trias.DataService.Tests.Helpers;
using Trias.DataService.v1_0;
using Trias.DataService.v1_0.DataModel;

namespace Trias.DataService.Tests
{
    [TestFixture]
    public class RequestTests
    {
        private const string KnownStationId = "de:08221:1160";

        [Test]
        public async Task LocationInformationRequest_Test()
        {
            //Arrange
            var client = new TriasServiceClient(ConfigHelper.TriasServiceUrl, ConfigHelper.TriasServiceRef);

            var input = new LocationInformationRequestStructure()
                .WithGeoRestriction(new GeoPositionStructure
                {
                    Longitude = 8.675760m,
                    Latitude = 49.404274m
                }, 100)
                .WithTypeRestriction(LocationTypeEnumeration.stop);

            //Act
            var result = await client.Request(input);

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ErrorMessage, Is.Null);
            Assert.That(result.Location, Is.Not.Null);
            Assert.That(result.Location.Length, Is.GreaterThan(0));
            Assert.IsInstanceOf<StopPointStructure>(result.Location[0].Location.Item);
            Assert.IsTrue(result.Location.Any(l =>
                ((StopPointStructure) l.Location.Item).StopPointRef.Value == KnownStationId));
        }

        [Test]
        public async Task StopEventResponseStructure_Test()
        {
            //Arrange
            string knownStationId = KnownStationId;
            var serviceClient = new TriasServiceClient(ConfigHelper.TriasServiceUrl, ConfigHelper.TriasServiceRef);

            var input = new StopEventRequestStructure()
                .SearchByStopPointRef(knownStationId);
            //Act
            var result = await serviceClient.Request(input);

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ErrorMessage, Is.Null);
            Assert.That(result.StopEventResult, Is.Not.Null);
            Assert.That(result.StopEventResult.Length, Is.GreaterThan(0));
        }

        //TODO: add test for all other supported methods
    }
}