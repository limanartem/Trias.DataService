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
        private readonly XmlSerializer _serializer = new XmlSerializer(typeof(DataModel.Trias));
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

                var obj = (DataModel.Trias) _serializer.Deserialize(responseStream);
                return ((ServiceDeliveryStructure) obj.Item).DeliveryPayload;
            }
        }

        private async Task<HttpResponseMessage> GetServiceResult(HttpClient httpClient, DataModel.Trias inputWrapper)
        {
            using (var streamContent = new MemoryStream())
            {
                _serializer.Serialize(streamContent, inputWrapper);
                streamContent.Position = 0;


                var result = await httpClient.PostAsync(
                    _triasServiceUri,
                    new StreamContent(streamContent));
                return result;
            }
        }
    }
}