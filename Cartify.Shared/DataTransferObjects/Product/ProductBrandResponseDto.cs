namespace Cartify.Shared.DataTransferObjects.Product;

public record ProductBrandResponseDto
{
    public int Id { get; init; }
    public required string Name { get; set; }
    public required string Slug { get; set; }
    public required string Image { get; set; }
    public DateTimeOffset? UpdatedAtUtc { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
}