using Cartify.Domain.Entities;
using Cartify.Domain.Interfaces;
using Cartify.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Cartify.Persistence
{
	internal class AppDataSeeder(AppDbContext context, ILogger<AppDataSeeder>? logger) : IAppDataSeeder
	{
		public async Task SeedAsync()
		{
			if (context is null)
			{
				return;
			}

			if (!await context.Set<Brand>().AsNoTracking().AnyAsync())
			{
				var brands = await DataSeeder.
					SeedItemsFromJson<Brand>(@"../Cartify.Persistence/AppData/DataSeedingSource/brands.json");
				if (brands is not null)
				{
					await context.Set<Brand>().AddRangeAsync(brands);
					await context.SaveChangesAsync();
				}
			}
			if (!await context.Set<Category>().AsNoTracking().AnyAsync())
			{

				var categories = await DataSeeder.
					SeedItemsFromJson<Category>(@"../Cartify.Persistence/AppData/DataSeedingSource/categories.json");

				if (categories is not null)
				{
					await context.Set<Category>().AddRangeAsync(categories);
					await context.SaveChangesAsync();
				}
			}
			if (!await context.Set<Product>().AsNoTracking().AnyAsync())
			{

				var products = await DataSeeder.
					SeedItemsFromJson<Product>(@"../Cartify.Persistence/AppData/DataSeedingSource/products.json");
				if (products is not null)
				{
					await context.Set<Product>().AddRangeAsync(products);
					await context.SaveChangesAsync();
				}
			}
		}
	}
}
