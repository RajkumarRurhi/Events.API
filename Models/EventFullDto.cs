using System;
using System.Collections.Generic;

namespace Events.API.Models
{
    public class EventFullDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string EventType { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }

        public ICollection<GuestModel> Guests { get; set; } = new List<GuestModel>();
    }
}
