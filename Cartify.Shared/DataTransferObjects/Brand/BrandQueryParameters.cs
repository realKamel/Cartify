namespace Cartify.Shared.DataTransferObjects.Brand;

public record BrandQueryParameters
{
    private const int MaxLimit = 40;

    private int _limit = 30;
    public string? Keyword { get; set; }

    public int Limit
    {
        get => _limit;
        set => _limit = value > MaxLimit ? MaxLimit : value;
    }

    public int? Page { get; set; } = 1;
}