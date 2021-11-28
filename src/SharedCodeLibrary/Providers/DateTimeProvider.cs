using SharedCodeLibrary.Interfaces;
using System;

namespace SharedCodeLibrary.Providers
{
    public class DateTimeProvider : IDateTimeProvider
    {
        private readonly string _dateTimeFormatting;

        public DateTimeProvider(string dateStringFormatting)
        {
            _dateTimeFormatting = dateStringFormatting;
        }

        public DateTime? ConvertFrom(string dateTimeString)
        {
            if (string.IsNullOrWhiteSpace(dateTimeString))
            {
                return null;
            }

            return DateTime.Parse(dateTimeString);
        }

        public string ConvertToDateString(DateTime? dateTime)
        {
            if (!dateTime.HasValue)
            {
                return null;
            }

            return dateTime.Value.ToString(_dateTimeFormatting);
        }

        public DateTime GetCurrentTimeUTC()
        {
            return DateTime.UtcNow;
        }
    }
}
