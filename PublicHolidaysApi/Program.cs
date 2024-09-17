using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using PublicHolidaysApi.Helpers;
using PublicHolidaysApi.Models;
using PublicHolidaysApi.Services;
using PublicHolidaysApi.Services.Api;

namespace PublicHolidaysApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(x =>
        {
            x.MapType<CountryCode>(() => new OpenApiSchema { Type = "string" });
        });
        builder.Services.AddControllers().AddJsonOptions(x =>
        {
            x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
        ConfigureCustomServices(builder.Services);
        
        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        
        app.Run();
    }

    private static void ConfigureCustomServices(IServiceCollection services)
    {
        services.AddHttpClient<IEnricoApiService, EnricoApiService>(client =>
        {
            client.BaseAddress = new Uri("https://kayaposoft.com/enrico/json/v3.0");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        });
        services.AddTransient<IHolidayService, HolidayService>();
    }
}