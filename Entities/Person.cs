using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Events.API.Entities
{
    public class Person
    {
        [Key]
        public string Email { get; set; }

        [Required, MaxLength(50)]
        public string FirstName { get; set; }

        [Required, MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        public DateTime DateOfBirth {  get; set; }

        public ICollection<PersonEvent> EventsToAttend { get; set; } = new List<PersonEvent>();
    }
}
