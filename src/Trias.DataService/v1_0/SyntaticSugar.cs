using Trias.DataService.v1_0.DataModel;

namespace Trias.DataService.v1_0
{
    /// <summary>
    /// Syntatic sugar class for incapsulating trias service input parameter complexity
    /// </summary>
    public static class SyntaticSugar
    {
        /// <summary>
        /// Defines geo restriction for request
        /// </summary>
        /// <param name="input"></param>
        /// <param name="geoPosition">Geo coordinates of the point to search around</param>
        /// <param name="radius">Radius restriction</param>
        /// <returns></returns>
        public static LocationInformationRequestStructure WithGeoRestriction(
            this LocationInformationRequestStructure input, GeoPositionStructure geoPosition, int radius)
        {
            input.Item = new InitialLocationInputStructure
            {
                GeoRestriction = new GeoRestrictionsStructure
                {
                    Item = new GeoCircleStructure
                    {
                        Center = geoPosition,
                        Radius = radius.ToString()
                    }
                }
            };
            return input;
        }

        /// <summary>
        /// Defines location type restriction
        /// </summary>
        /// <param name="input"></param>
        /// <param name="types">Types to be included in the search result</param>
        /// <returns></returns>
        public static LocationInformationRequestStructure WithTypeRestriction(
            this LocationInformationRequestStructure input, params LocationTypeEnumeration[] types)
        {
            input.Restrictions = new LocationParamStructure
            {
                Type = types
            };
            return input;
        }


        /// <summary>
        /// Specifies search stop point reference
        /// </summary>
        /// <param name="input"></param>
        /// <param name="stopPointRef">Id of the stop</param>
        /// <returns></returns>
        public static StopEventRequestStructure SearchByStopPointRef(this StopEventRequestStructure input, string stopPointRef)
        {
            input.Location = new LocationContextStructure()
            {
                Item = new LocationRefStructure()
                {
                    Item = new StopPointRefStructure()
                    {
                        Value = stopPointRef
                    }
                }
            };

            return input;
        }

        /// <summary>
        /// Specifies search stop point reference
        /// </summary>
        /// <param name="input"></param>
        /// <param name="stopPointRef">Id of the stop</param>
        /// <returns></returns>
        public static LocationInformationRequestStructure SearchByStopPointRef(this LocationInformationRequestStructure input, string stopPointRef)
        {
            input.Item = new LocationRefStructure()
            {
                Item = new StopPointRefStructure()
                {
                    Value = stopPointRef
                }
            };

            return input;
        }
    }
}
