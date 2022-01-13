using System;
using System.ComponentModel.DataAnnotations;

namespace Events.API.Models
{
    public abstract class GuestForManuplation
    {
        [Required]
        public string Email { get; set; }

        [Required, MaxLength(50)]
        public string FirstName { get; set; }

        [Required, MaxLength(50)]
        public string LastName { get; set; }

        public virtual DateTime DateOfBirth { get; set; }
    }
}
