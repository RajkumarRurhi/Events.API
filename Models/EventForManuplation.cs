using Events.API.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Events.API.Models
{
    [ValidateEventTimings(ErrorMessage = "Start date time can not be later than end date time.")]
    public abstract class EventForManuplation
    {
        [Required, MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public virtual string Description { get; set; }
        public virtual string EventType { get; set; }

        [Required]
        public DateTime StartDateTime { get; set; }

        [Required]
        public DateTime EndDateTime { get; set; }

        public ICollection<GuestForAddingModel> Guests { get; set; } = new List<GuestForAddingModel>();
    }
}
