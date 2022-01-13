using System.Collections.Generic;

namespace Events.API.Helpers
{
    public class PropertyMappingValue
    {
        public IEnumerable<string> DestinationProperties { get; private set; }
        public bool ReverseOrder { get; private set; }

        public PropertyMappingValue(IEnumerable<string> destinationProperties, bool reverseOrder = false)
        {
            DestinationProperties = destinationProperties;
            ReverseOrder = reverseOrder;
        }
    }
}
