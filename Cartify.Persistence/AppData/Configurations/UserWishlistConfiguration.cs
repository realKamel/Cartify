using Cartify.Domain.Entities.UserRelatedEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cartify.Persistence.AppData.Configurations
{
	internal class UserWishlistConfiguration : IEntityTypeConfiguration<UserWishlist>
	{
		public void Configure(EntityTypeBuilder<UserWishlist> builder)
		{
			//the constraints must be fixed
			builder.HasKey(u => u.Id);
			builder.HasIndex(u => u.UserId)
				.IsUnique();
			
			builder
				.HasMany(p => p.WishlistProducts)
				.WithOne(p => p.UserWishlist)
				.HasForeignKey(p => p.UserWishlistId);
		}
	}
}
