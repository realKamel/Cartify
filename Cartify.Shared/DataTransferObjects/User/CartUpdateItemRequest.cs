using System.ComponentModel.DataAnnotations;

namespace Cartify.Shared.DataTransferObjects.User;

public record CartUpdateItemRequest
{
	[Required]
	public required string ItemId { get; set; }


	[Required]
	[Range(0, int.MaxValue)]
	public int NewCount { get; set; }
}
