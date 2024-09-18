using Microsoft.AspNetCore.Mvc;
using PublicHolidaysApi.Enums;
using PublicHolidaysApi.Models;
using PublicHolidaysApi.Services;

namespace PublicHolidaysApi.Controllers;


/// <summary>
/// Controller for managing public holiday-related requests.
/// </summary>
[ApiController]
[Route("api")]
public class PublicHolidaysController : ControllerBase
{
    private readonly IHolidayService _holidayService;

    /// ctor
    public PublicHolidaysController(IHolidayService holidayService)
    {
        _holidayService = holidayService;
    }

    /// <summary>
    /// Gets a list of supported countries for public holidays.
    /// </summary>
    /// <returns>A list of supported countries and their codes.</returns>
    [HttpGet("SupportedCountries")]
    public async Task<ActionResult<SupportedCountriesDto>> GetSupportedCountries()
    {
        try
        {
            var countries = await _holidayService.GetSupportedCountriesAsync();
            return Ok(countries);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// Gets the status of a specific day for a given country.
    /// </summary>
    /// <param name="countryCode">Country code for which to retrieve the day status. Format: "xxx", must be letters.</param>
    /// <param name="date">The date for which to retrieve the day status. Format: "yyyy-MM-dd".</param>
    /// <returns>The status of the specified day. Possible values are Workday, PublicHoliday and FreeDay. </returns>
    [HttpGet("DayStatus/{countryCode}/{date:datetime}")]
    public async Task<ActionResult<DayStatus>> GetDayStatus(CountryCode countryCode, DateOnly date)
    {
        try
        {
            var dayStatus = await _holidayService.GetSpecificDayStatusAsync(countryCode, date);
            return Ok(dayStatus);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
    
    /// <summary>
    /// Gets holidays for a specific country and year, grouped by month.
    /// </summary>
    /// <param name="countryCode">Country code for which to retrieve the day status. Format: "xxx", must be letters.</param>
    /// <param name="year">Year for which to retrieve holidays.</param>
    /// <returns>A grouped list of holidays.</returns>
    [HttpGet("Holidays/{countryCode}/{year:int}")]
    public async Task<ActionResult<GroupedHolidaysDto>> GetHolidaysAsync(CountryCode countryCode, int year)
    {
        try
        {
            var groupedHolidays = await _holidayService.GetHolidaysAsync(countryCode, year);
            return Ok(groupedHolidays);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
    
    /// <summary>
    /// Gets the maximum number of consecutive free days in a requested year for a given country.
    /// </summary>
    /// <param name="countryCode">Country code for which to retrieve the day status. Format: "xxx", must be letters.</param>
    /// <param name="year">Year for which to calculate free days.</param>
    /// <returns>The maximum number of consecutive free days in a year.</returns>
    [HttpGet("MaximumConsecutiveFreeDays/{countryCode}/{year:int}")]
    public async Task<ActionResult<int>> GetMaximumFreeDays(CountryCode countryCode, int year)
    {
        try
        {
            var maximumFreeDays = await _holidayService.GetMaximumFreeDays(countryCode, year);
            return Ok(maximumFreeDays);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    private ActionResult HandleException(Exception ex)
    {
        if (ex is HttpRequestException httpRequestException && httpRequestException.StatusCode.HasValue)
        {
            return StatusCode((int)httpRequestException.StatusCode, httpRequestException.Message);
        }
        
        return StatusCode(500, ex.Message);
    }
}