using Scalar.AspNetCore;
using Serilog;

namespace Cartify.Web;

public class Program
{
    public static void Main(string[] args)
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

            //full Serilog setup after startup
            // no use for builder.Logging.ClearProviders(); and builder.Logging.AddSerilog(logger);
            // here as UseSerilog handles that.
            builder.Host.UseSerilog((context, services, configuration) =>
                configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
            );
            
            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            var app = builder.Build();
            Log.Information("Web application built successfully.");

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();

            app.UseHsts();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly!");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}