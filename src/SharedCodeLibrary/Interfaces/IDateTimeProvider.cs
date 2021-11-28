using System;

namespace SharedCodeLibrary.Interfaces
{
    public interface IDateTimeProvider
    {
        string ConvertToDateString(DateTime? dateTime);

        DateTime? ConvertFrom(string dateTimeString);

        DateTime GetCurrentTimeUTC();
    }
}
