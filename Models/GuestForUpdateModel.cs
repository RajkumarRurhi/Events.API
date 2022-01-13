using System;
using System.ComponentModel.DataAnnotations;

namespace Events.API.Models
{
    public class GuestForUpdateModel : GuestForManuplation
    {
        [Required]
        public override DateTime DateOfBirth { get => base.DateOfBirth; set => base.DateOfBirth = value; }
    }
}
