using Cartify.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cartify.Persistence.AppData.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.Id);

        builder
            .HasMany(p => p.BrandCategories)
            .WithOne(bc => bc.Category)
            .HasForeignKey(bc => bc.CategoryId);

        builder.HasData(CategoriesSeed().Result ?? []);
    }

    private static async Task<IEnumerable<Category>?> CategoriesSeed()
    {
        return await DataSeeder.SeedItemsFromJson<Category>(
            @"../Cartify.Persistence/AppData/DataSeedingSource/categories.json");
    }
}