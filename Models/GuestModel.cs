using System;
using System.ComponentModel.DataAnnotations;

namespace Events.API.Models
{
    public class GuestModel
    {
        public string Email { get; set; }

        [Required, MaxLength(101)]
        public string Name { get; set; }

        [Required]
        public int Age { get; set; }
    }
}
