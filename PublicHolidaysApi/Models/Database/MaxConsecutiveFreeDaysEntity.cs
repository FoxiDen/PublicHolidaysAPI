using System.ComponentModel.DataAnnotations.Schema;

namespace PublicHolidaysApi.Models.Database;

public class MaxConsecutiveFreeDaysEntity
{
    public required string CountryCode { get; set; }
    public required int Year { get; set; }
    public required int MaxConsecutiveDays { get; set; }
}