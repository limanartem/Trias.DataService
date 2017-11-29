using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Trias.DataService.v1_0.DataModel;

namespace Trias.DataService.v1_0
{
    /// <summary>
    /// Client for interacting with a Trias service interface v1.0
    /// </summary>
    public sealed class TriasServiceClient
    {
        private readonly string _triasServiceRequestorRef;

        private static readonly Lazy<XmlSerializer> _serializer =
            new Lazy<XmlSerializer>(() => new XmlSerializer(typeof(DataModel.Trias)));

        private readonly Uri _triasServiceUri;

        public TriasServiceClient(Uri serviceUri, string requestorRef)
        {
            if (string.IsNullOrEmpty(requestorRef)) throw  new ArgumentNullException(nameof(requestorRef));

            _triasServiceRequestorRef = requestorRef;
            _triasServiceUri = serviceUri;
        }


        public async Task<LocationInformationResponseStructure> Request(LocationInformationRequestStructure input)
        {
            return await Request<LocationInformationResponseStructure>(input);
        }
        public async Task<StopEventResponseStructure> Request(StopEventRequestStructure input)
        {
            return await Request<StopEventResponseStructure>(input);
        }

        public async Task<BookingInfoResponseStructure> Request(BookingInfoRequestStructure input)
        {
            return await Request<BookingInfoResponseStructure>(input);
        }

        public async Task<ConnectionDemandDeleteResponseStructure> Request(ConnectionDemandDeleteRequestStructure input)
        {
            return await Request<ConnectionDemandDeleteResponseStructure>(input);
        }
   
        public async Task<ConnectionDemandResponseStructure> Request(ConnectionDemandRequestStructure input)
        {
            return await Request<ConnectionDemandResponseStructure>(input);
        }

        public async Task<ConnectionReportResponseStructure> Request(ConnectionReportRequestStructure input)
        {
            return await Request<ConnectionReportResponseStructure>(input);
        }

        public async Task<ConnectionStatusResponseStructure> Request(ConnectionStatusRequestStructure input)
        {
            return await Request<ConnectionStatusResponseStructure>(input);
        }

        public async Task<FacilityStatusReportResponseStructure> Request(FacilityStatusReportStructure input)
        {
            return await Request<FacilityStatusReportResponseStructure>(input);
        }

        public async Task<FaresResponseStructure> Request(FaresRequestStructure input)
        {
            return await Request<FaresResponseStructure>(input);
        }

        public async Task<GeoCoordinatesResponseStructure> Request(GeoCoordinatesRequestStructure input)
        {
            return await Request<GeoCoordinatesResponseStructure>(input);
        }

        public async Task<ImageCoordinatesResponseStructure> Request(ImageCoordinatesRequestStructure input)
        {
            return await Request<ImageCoordinatesResponseStructure>(input);
        }

        public async Task<IndividualRouteResponseStructure> Request(IndividualRouteRequestStructure input)
        {
            return await Request<IndividualRouteResponseStructure>(input);
        }

        public async Task<MapServiceResponseStructure> Request(MapServiceRequestStructure input)
        {
            return await Request<MapServiceResponseStructure>(input);
        }

        public async Task<PersonalisationResponseStructure> Request(PersonalisationRequestStructure input)
        {
            return await Request<PersonalisationResponseStructure>(input);
        }

        public async Task<PositioningResponseStructure> Request(PositioningRequestStructure input)
        {
            return await Request<PositioningResponseStructure>(input);
        }

        public async Task<ServiceRegisterResponseStructure> Request(ServiceRegisterRequestStructure input)
        {
            return await Request<ServiceRegisterResponseStructure>(input);
        }

        public async Task<StopRequestResponseStructure> Request(StopRequestRequestStructure input)
        {
            return await Request<StopRequestResponseStructure>(input);
        }

        public async Task<TripInfoResponseStructure> Request(TripInfoRequestStructure input)
        {
            return await Request<TripInfoResponseStructure>(input);
        }

        public async Task<TripResponseStructure> Request(TripRequestStructure input)
        {
            return await Request<TripResponseStructure>(input);
        }

        public async Task<VehicleDataResponseStructure> Request(VehicleDataRequestStructure input)
        {
            return await Request<VehicleDataResponseStructure>(input);
        }


        private async Task<T> Request<T>(object input)
        {
            using (var httpClient = new HttpClient())
            {
                var wrappedInput = WrapInput(input);

                using (var result = await GetServiceResult(httpClient, wrappedInput))
                {
                    var responsePayload = await GetResponsePayload(result);
                    return (T) responsePayload.Item;
                }
            }
        }

        private DataModel.Trias WrapInput(object input)
        {
            return new DataModel.Trias()
            {
                Item = new ServiceRequestStructure()
                {
                    RequestTimestamp = System.DateTime.Now,
                    RequestorRef = new ParticipantRefStructure1() {Value = _triasServiceRequestorRef},
                    RequestPayload = new RequestPayloadStructure()
                    {
                        Item = input
                    }
                }
            };
        }

        private async Task<DeliveryPayloadStructure> GetResponsePayload(HttpResponseMessage result)
        {
            var stringResponse = await result.Content.ReadAsStringAsync();

            using (var responseStream = new MemoryStream())
            using (var streamWriter = new StreamWriter(responseStream))
            {

                await streamWriter.WriteAsync(stringResponse);
                streamWriter.Flush();
                responseStream.Position = 0;

                var obj = (DataModel.Trias) _serializer.Value.Deserialize(responseStream);
                return ((ServiceDeliveryStructure) obj.Item).DeliveryPayload;
            }
        }

        private async Task<HttpResponseMessage> GetServiceResult(HttpClient httpClient, DataModel.Trias inputWrapper)
        {
            using (var streamContent = new MemoryStream())
            {
                _serializer.Value.Serialize(streamContent, inputWrapper);
                streamContent.Position = 0;


                var result = await httpClient.PostAsync(
                    _triasServiceUri,
                    new StreamContent(streamContent));
                return result;
            }
        }
    }
}