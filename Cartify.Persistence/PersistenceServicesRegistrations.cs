using Cartify.Domain.Entities;
using Cartify.Domain.Interfaces;
using Cartify.Persistence.DbContexts;
using Cartify.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Cartify.Persistence;

public static class PersistenceServicesRegistrations
{
	public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
	{
		services.AddHttpContextAccessor();
		services.AddScoped<AuditInterceptor>();

		//dbContexts configuration
		services.AddDbContext<AppDbContext>((services, options) =>
		{
			var auditInterceptor = services.GetRequiredService<AuditInterceptor>();

			options
				.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
				.AddInterceptors(auditInterceptor);

			if (environment.IsDevelopment())
			{
				options.LogTo(Console.WriteLine, LogLevel.Information);
				options.EnableSensitiveDataLogging();
			}
		});

		services.AddDbContext<IdentityContext>(options =>
		{
			options.UseNpgsql(configuration.GetConnectionString("IdentityConnection"));
		});

		services
			.AddIdentityCore<AppUser>(options =>
			{
				options.User.RequireUniqueEmail = true;
				options.Password.RequiredLength = 8;
				options.Password.RequireDigit = true;
				options.Password.RequireLowercase = false;
			})
			.AddRoles<AppRole>()
			.AddEntityFrameworkStores<IdentityContext>();

		services.AddScoped<IUnitOfWork, UnitOfWork>();
		services.AddScoped<IIdentityDataSeeder, IdentityDataSeeder>();
		services.AddScoped<IAppDataSeeder, AppDataSeeder>();
		services.AddScoped<ICartRepository, CartRepository>();

		//services.AddSingleton<IConnectionMultiplexer>((_) =>
		//{
		//    return ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisConnection")!);
		//});

		services.AddSingleton<IConnectionMultiplexer>(sp =>
		{

			// Retrieve the connection string
			string redisConnectionString = configuration.GetConnectionString("RedisConnection")
										?? throw new InvalidOperationException("RedisConnection string is not configured.");

			try
			{
				// 1. Parse the string to get host/port/db settings
				var options = ConfigurationOptions.Parse(redisConnectionString);

				// 2. Apply custom options for robustness
				options.AsyncTimeout = 5000;
				options.AbortOnConnectFail = false;


				// 3. Connect to Redis
				return ConnectionMultiplexer.Connect(options);
			}
			catch (Exception)
			{
				throw;
			}
		});
	}
}