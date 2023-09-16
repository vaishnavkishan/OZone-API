using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OZone.Api.Domain;
using OZone.Api.Integrations;
using OZone.Api.Services;

namespace OZone.Api.Extensions;

public static class Dependencies
{
    public static void RegisterDependencies(this IServiceCollection services)
    {
        services.AddControllers();

        services.AddDatabase();
        
        services.AddSwagger();
        
        services.AddCors();

        services.AddServices();
    }

    private static void AddSwagger(this IServiceCollection services)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "OZone Event API",
                Description = "An API for managing Events"
            });

            // using System.Reflection;
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });
    }

    private static void AddDatabase(this IServiceCollection services)
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        var DbPath = Path.Join(path, "event.db");
        services.AddDbContext<EventContext>(opt =>
            opt.UseSqlite($"Data Source={DbPath}"));
    }
    
    private static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IEmailSender, SendGridEmail>();
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IOpenAiIntegration, OpenAiIntegration>();
    }

}