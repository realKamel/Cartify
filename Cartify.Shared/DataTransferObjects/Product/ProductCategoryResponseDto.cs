namespace Cartify.Shared.DataTransferObjects.Product;

public record ProductCategoryResponseDto
{
    public int Id { get; init; }
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset? UpdatedAtUtc { get; set; }
    public required string Image { get; set; }
}