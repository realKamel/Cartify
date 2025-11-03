using Cartify.Shared.DataTransferObjects;

namespace Cartify.Shared;

public record ProductQueryParameters
{
    private const int MaxLimit = 40;

    private int _limit = 30;
    public string? Keyword { get; set; }
    public OrderByEnum? OrderBy { get; set; }
    public OrderByEnum? OrderByDesc { get; set; }

    public int Limit
    {
        get => _limit;
        set => _limit = value > MaxLimit ? MaxLimit : value;
    }

    public int? Page { get; set; } = 1;
    public int? Brand { get; set; }
    public int? Category { get; set; }
}