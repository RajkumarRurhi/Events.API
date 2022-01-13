using System;

namespace Events.API.Helpers
{
    public static class DateTimeExtension
    {
        public static int GetAge(this DateTime dateTime)
        {
            var currentDate = DateTime.Now;
            int age = currentDate.Year - dateTime.Year;

            if (currentDate < dateTime.AddYears(age))
            {
                age--;
            }

            return age;
        }
    }
}
