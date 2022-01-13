using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Events.API.Models
{
    public class EventModel
    {
        public Guid Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; }

        [Required, MaxLength(1000)]
        public string Description { get; set; }
        public string EventType { get; set; }

        [Required]
        public DateTime StartDateTime { get; set; }

        [Required]
        public DateTime EndDateTime { get; set; }

        public string Address { get; set; }

        public ICollection<GuestModel> Guests { get; set; } = new List<GuestModel>();
    }
}
