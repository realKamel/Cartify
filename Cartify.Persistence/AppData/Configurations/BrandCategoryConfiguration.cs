using Cartify.Domain.Entities.JoinEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cartify.Persistence.AppData.Configurations;

public class BrandCategoryConfiguration : IEntityTypeConfiguration<BrandCategory>
{
    public void Configure(EntityTypeBuilder<BrandCategory> builder)
    {
        builder.HasKey(e => new { e.BrandId, e.CategoryId });

        builder.HasOne(x => x.Brand)
            .WithMany(x => x.BrandCategories)
            .HasForeignKey(x => x.BrandId);

        builder.HasOne(x => x.Category)
            .WithMany(x => x.BrandCategories)
            .HasForeignKey(x => x.CategoryId);
    }
}