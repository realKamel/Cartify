using System.Text.Json;
using Cartify.Domain.Entities;
using Cartify.Domain.Interfaces;
using Cartify.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Cartify.Persistence;

public sealed class DataSeeder(AppDbContext dbContext, ILogger<DataSeeder> logger) : IDataSeeder
{
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        // to accept pascalCase into CamelCase 
        PropertyNameCaseInsensitive = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true
    };

    private async Task<IEnumerable<T>?> SeedItemsFromJson<T>(string filePath)
    {
        try
        {
            var itemsFileStream = File.OpenRead(filePath);
            var items = await JsonSerializer.DeserializeAsync<IEnumerable<T>>(itemsFileStream,
                _jsonOptions);
            return items;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            logger.LogError("Error in Reading file {error}", e.Message);
            throw;
        }
    }

    public async Task SeedDataAsync()
    {
        try
        {
            var pendingMigration = await dbContext.Database.GetPendingMigrationsAsync();
            if (pendingMigration.Any())
            {
                await dbContext.Database.MigrateAsync();
            }

            if (!dbContext.Set<Product>().Any())
            {
                var productsSeed =
                    await SeedItemsFromJson<Product>(
                        @"../Cartify.Persistence/AppData/DataSeedingSource/products.json");
                if (productsSeed is null)
                {
                    throw new Exception("No products were found in seeding files");
                }

                await dbContext.Set<Product>().AddRangeAsync(productsSeed);
            }

            if (!dbContext.Set<Brand>().Any())
            {
                var brandsSeed =
                    await SeedItemsFromJson<Brand>(
                        @"../Cartify.Persistence/AppData/DataSeedingSource/brands.json");
                if (brandsSeed is null)
                {
                    throw new Exception("No brands were found in seeding files.");
                }

                await dbContext.Set<Brand>().AddRangeAsync(brandsSeed);
            }

            if (!dbContext.Set<Category>().Any())
            {
                var categoriesSeed =
                    await SeedItemsFromJson<Category>(
                        @"../Cartify.Persistence/AppData/DataSeedingSource/categories.json");
                if (categoriesSeed is null)
                {
                    throw new Exception("No Categories were found in seeding files.");
                }

                await dbContext.Set<Category>().AddRangeAsync(categoriesSeed);
            }

            await dbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            logger.LogError("Error in App Data Seeding, {message}", e.Message.ToString());
            throw;
        }
    }
}