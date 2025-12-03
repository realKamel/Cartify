using Cartify.Domain.Entities.JoinEntities;

namespace Cartify.Domain.Entities;

public class Product : BaseEntity<int>
{
	public required string Title { get; set; }
	public required string Slug { get; set; }
	public string? Description { get; set; }
	public string ImageCover { get; set; }
	public IList<string>? Images { get; set; }
	public int Sold { get; set; }
	public double RatingsAverage { get; set; }
	public int RatingsQuantity { get; set; }
	public decimal Price { get; set; }
	public int Quantity { get; set; }

	//Product,Brand relation
	public int BrandId { get; set; }
	public Brand Brand { get; set; }

	//product,Category relation
	public int CategoryId { get; set; }
	public Category Category { get; set; }

	public IList<WishlistedProduct>? WishlistedProducts { get; set; } = [];
}