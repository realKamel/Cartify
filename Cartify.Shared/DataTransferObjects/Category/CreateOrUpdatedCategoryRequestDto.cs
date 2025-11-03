using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Cartify.Shared.DataTransferObjects.Category;

public class CreateOrUpdatedCategoryRequestDto
{
    [Required]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "Brand name MUST be between 3 and 200 characters")]
    public required string Name { get; set; }

    [Required] public required IFormFile Image { get; set; }

    //TODO I Should implement method to update the related Data

    // public ICollection<Product>? Products { get; set; }
    // public ICollection<BrandCategory> BrandCategories { get; set; }
}