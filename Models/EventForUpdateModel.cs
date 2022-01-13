using System.ComponentModel.DataAnnotations;

namespace Events.API.Models
{
    public class EventForUpdateModel : EventForManuplation
    {
        [Required]
        public override string Description { get => base.Description; set => base.Description = value; }
        [Required]
        public override string EventType { get => base.EventType; set => base.EventType = value; }
    }
}
