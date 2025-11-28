using Cartify.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cartify.Persistence.AppData.Configurations;

public class BrandConfiguration : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder.HasKey(b => b.Id);

        builder
            .HasMany(p => p.BrandCategories)
            .WithOne(bc => bc.Brand)
            .HasForeignKey(bc => bc.BrandId);
    }
}