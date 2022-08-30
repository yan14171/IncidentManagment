using IncidentManagment.Data;
using IncidentManagment.Logic.Interfaces;
using IncidentManagment.Logic.Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;

Log.Logger = new LoggerConfiguration().Enrich.FromLogContext().WriteTo.Console().CreateBootstrapLogger();

try
{
    Run();
}
catch(Exception ex)
{
    Log.Fatal(ex, "Host terminated due to an unhandled exception");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}
return 0;


void Run()
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, _, conf) =>
    conf.ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext());

    builder.Services.AddDbContext<IncidentContext>(options =>
                    options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"])
                           .UseLoggerFactory(LoggerFactory.Create(n => n.AddConsole())));

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "Incident API",
            Description = "A test task, a Web API for managing incidents",
            TermsOfService = new Uri("https://terms"),
            Contact = new OpenApiContact
            {
                Name = "Contact",
                Url = new Uri("https://contact")
            },
            License = new OpenApiLicense
            {
                Name = "License",
                Url = new Uri("https://license")
            }
        });

        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    });
    builder.Services.AddLogging(s => s.AddSerilog(Log.Logger));
    builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
    builder.Services.AddCustomServices();

    //builder.Services.AddLogging(s => s.AddSerilog());
    
    var app = builder.Build();

    app.UseSerilogRequestLogging(conf => 
        conf.MessageTemplate = "REQUEST {RequestMethod} to path: {RequestPath} from {UserId} responded in {Elapsed}ms with {StatusCode}");

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    if (app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                var pd = new ProblemDetails
                {
                    Type = "https://demo.api.com/errors/internal-server-error",
                    Title = "An unrecoverable error occurred",
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = exceptionHandlerPathFeature?.Error.Message,
                };
                pd.Extensions.Add("RequestId", context.TraceIdentifier);
                await context.Response.WriteAsJsonAsync(pd, pd.GetType(), null, contentType: "application/problem+json");
            });
        });
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}


public static class ServiceCollectionExtenstions
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        services.AddScoped<IIncidentService, IncidentService>();
        services.AddScoped<IContactService, ContactService>();
        services.AddScoped<IAccountService, AccountService>();

        return services;
    }
}
