using System;

namespace Events.API.Entities
{
    public class PersonEvent
    {
        public Guid EventId { get; set; }
        public Event Event { get; set; }

        public string PersonEmail { get; set; }
        public Person Person { get; set; }

    }
}
