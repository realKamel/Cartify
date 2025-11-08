using Asp.Versioning;
using Cartify.Domain.Interfaces;
using Cartify.Persistence;
using Cartify.Services;
using Cartify.Web.WebAppHelpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
//using Scalar.AspNetCore;
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
            builder.Services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            }).AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            });

            //Persistence Layer Services
            builder.Services.AddPersistenceServices(builder.Configuration, builder.Environment);

            //Service Layer Services
            builder.Services.AddServiceLayerServices(builder.Configuration);

            //Global Exception Handling
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddScoped<UseEndpointChecker>();
            builder.Services.AddProblemDetails();

            // HTTPS enforce
            builder.Services.AddHsts(options =>
            {
                options.Preload = true; // Indicates the site should be included in browser preload lists
                options.IncludeSubDomains = true; // Applies the HSTS policy to all subdomains
                options.MaxAge = TimeSpan.FromDays(365); // Browser should enforce HTTPS for 1 year
            });

            var app = builder.Build();

            Log.Information("Web application built successfully.");

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
                app.UseExceptionHandler();
                app.UseMiddleware<UseEndpointChecker>();
            }

            app.UseSerilogRequestLogging();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();


            if (app.Environment.IsDevelopment())
            {
                using (var scope = app.Services.CreateScope())
                {
                    var identitySeed = scope.ServiceProvider.GetRequiredService<IIdentityDataSeeder>();
                    await identitySeed.SeedRolesAsync();
                    await identitySeed.SeedUsersAsync();
                }
            }

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