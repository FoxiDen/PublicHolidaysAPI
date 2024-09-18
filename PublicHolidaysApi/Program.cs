using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PublicHolidaysApi.Data;
using PublicHolidaysApi.Models;
using PublicHolidaysApi.Services;
using PublicHolidaysApi.Services.Api;
using PublicHolidaysApi.Services.Database;

namespace PublicHolidaysApi;

/// <summary>
/// The entry point for the application and the main configuration class.
/// </summary>
public class Program
{
    /// <summary>
    /// The main method that configures and runs the web application.
    /// </summary>
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        
        builder.Services.AddSwaggerGen(x =>
        {
            x.MapType<CountryCode>(() => new OpenApiSchema { Type = "string" });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            x.IncludeXmlComments(xmlPath);
        });
        
        builder.Services.AddControllers().AddJsonOptions(x =>
        {
            x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        
        ConfigureCustomServices(builder.Services);
        
        var app = builder.Build();
        
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();
        app.UseAuthorization();
        
        app.MapControllers();
        
        app.Run();
    }

    internal static void ConfigureCustomServices(IServiceCollection services)
    {
        services.AddHttpClient<IEnricoApiService, EnricoApiService>(client =>
        {
            client.BaseAddress = new Uri("https://kayaposoft.com/enrico/json/v3.0");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        });
        services.AddTransient<IHolidayService, HolidayService>();
        services.AddTransient<IDatabaseService, DatabaseService>();
    }
}