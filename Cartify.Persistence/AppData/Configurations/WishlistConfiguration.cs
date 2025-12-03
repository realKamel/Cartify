using Cartify.Domain.Entities.UserRelatedEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cartify.Persistence.AppData.Configurations
{
	internal class WishlistConfiguration : IEntityTypeConfiguration<Wishlist>
	{
		public void Configure(EntityTypeBuilder<Wishlist> builder)
		{
			//the constraints must be fixed
			builder.HasKey(u => u.Id);
			builder.HasIndex(u => u.UserId)
				.IsUnique();

			builder
				.HasMany(w => w.Products)
				.WithOne(p => p.Wishlist)
				.HasForeignKey(p => p.WishlistId);
		}
	}
}
