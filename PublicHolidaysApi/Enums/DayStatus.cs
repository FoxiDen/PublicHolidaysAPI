namespace PublicHolidaysApi.Enums;

/// <summary>
/// Status of a day.
/// </summary>
public enum DayStatus
{
    /// <summary>
    /// The day is a free day, like weekend.
    /// </summary>
    FreeDay,

    /// <summary>
    /// The day is a work day.
    /// </summary>
    WorkDay,

    /// <summary>
    /// The day is a public holiday.
    /// </summary>
    PublicHoliday
}