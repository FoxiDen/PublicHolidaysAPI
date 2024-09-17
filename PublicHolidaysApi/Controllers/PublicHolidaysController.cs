using Microsoft.AspNetCore.Mvc;
using PublicHolidaysApi.Enums;
using PublicHolidaysApi.Models;
using PublicHolidaysApi.Services;

namespace PublicHolidaysApi.Controllers;

[ApiController]
[Route("api")]
public class PublicHolidaysController : ControllerBase
{
    private readonly IHolidayService _holidayService;

    public PublicHolidaysController(IHolidayService holidayService)
    {
        _holidayService = holidayService;
    }

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