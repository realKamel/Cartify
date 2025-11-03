using System.ComponentModel.DataAnnotations;
using Cartify.Shared.DataTransferObjects.Brand;
using Microsoft.AspNetCore.Http;

namespace Cartify.Shared.DataTransferObjects.Product;

public record CreateOrUpdateProductRequestDto
{
	[Required]
	[StringLength(200, MinimumLength = 3, ErrorMessage = "Product Title Must be  between 3 and 200 characters")]
	public required string Title { get; set; }

	[StringLength(1000, MinimumLength = 3, ErrorMessage = "Description Must be  between 3 and 1000 characters")]
	public string? Description { get; set; }

	[Required]
	public required IFormFile ImageCover { get; set; }

	public List<IFormFile>? Images { get; set; }

	[Required]
	[Range(0, int.MaxValue, ErrorMessage = "Price Must be between 0 and 2,147,483,647")]
	public decimal Price { get; set; }

	[Required]
	[Range(0, int.MaxValue, ErrorMessage = "Quantity Must be between 0 and 2,147,483,647")]
	public int Quantity { get; set; }

	public int RatingsCount { get; set; }

	// //Product,Brand relation
	public int BrandId { get; set; }

	// //product,Category relation
	public int CategoryId { get; set; }
}