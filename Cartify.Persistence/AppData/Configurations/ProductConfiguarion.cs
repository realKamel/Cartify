using Cartify.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cartify.Persistence.AppData.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
	public void Configure(EntityTypeBuilder<Product> builder)
	{
		builder.HasKey(x => x.Id);
		builder.Property(x => x.Price).HasColumnType("decimal(10,2)");

		builder
			.HasOne(x => x.Brand)
			.WithMany(x => x.Products)
			.HasForeignKey(x => x.BrandId);

		builder
			.HasOne(x => x.Category)
			.WithMany(x => x.Products)
			.HasForeignKey(x => x.CategoryId);

		builder
			.HasMany(p => p.WishlistedProducts)
			.WithOne(p => p.Product)
			.HasForeignKey(p => p.ProductId);
	}
}