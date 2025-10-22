using Cartify.Domain.Interfaces;
using Cartify.Persistence;
using Scalar.AspNetCore;
using Serilog;

namespace Cartify.Web;

public class Program
{
    public static async Task Main(string[] args)
    {
        //initial logging for application startup
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateBootstrapLogger();
        try
        {
            Log.Information("Starting web host.");
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            // Full Serilog setup after startup
            // no use for builder.Logging.ClearProviders(); and builder.Logging.AddSerilog(logger);
            // as UseSerilog handles that.
            builder.Host.UseSerilog((context, services, configuration) =>
                configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
            );

            // Add services to the DI container.
            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            //Persistence Layer Services
            builder.Services.AddPersistenceServices(builder.Configuration);

            // HTTPS enforce
            builder.Services.AddHsts(options =>
            {
                options.Preload = true; // Indicates the site should be included in browser preload lists
                options.IncludeSubDomains = true; // Applies the HSTS policy to all subdomains
                options.MaxAge = TimeSpan.FromDays(365); // Browser should enforce HTTPS for 1 year
            });

            var app = builder.Build();

            Log.Information("Web application built successfully.");


            Log.Information("Start DataSeeding");

            using (var scope = app.Services.CreateScope())
            {
                try
                {
                    var dataSeeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();

                    // 1. MUST AWAIT the async call here
                    await dataSeeder.SeedDataAsync();
                }
                catch (Exception ex)
                {
                    // 2. Add logging to catch any errors that occur during seeding
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }
            else
            {
                app.UseHsts();
            }

            app.UseSerilogRequestLogging();

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            await app.RunAsync();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly!");
        }
        finally
        {
            await Log.CloseAndFlushAsync();
        }
    }
}