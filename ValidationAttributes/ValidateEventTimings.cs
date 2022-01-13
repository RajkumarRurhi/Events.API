using Events.API.Entities;
using Events.API.Models;
using System.ComponentModel.DataAnnotations;

namespace Events.API.ValidationAttributes
{
    public class ValidateEventTimings: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            EventForManuplation evnt = validationContext.ObjectInstance as EventForManuplation;

            if(evnt.StartDateTime.CompareTo(evnt.EndDateTime) >= 0)
            {
                return new ValidationResult(ErrorMessage, new[] { nameof(EventForManuplation) });
            }

            return ValidationResult.Success;
        }
    }
}
