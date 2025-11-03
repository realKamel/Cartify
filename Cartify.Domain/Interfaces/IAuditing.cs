namespace Cartify.Domain.Interfaces;

public interface IAuditing
{
    public DateTime CreatedAtUtc { get; set; }
    public Guid CreatedBy { get; set; }

    public DateTime? UpdatedAtUtc { get; set; }
    public Guid? UpdatedBy { get; set; }

    public DateTime? DeletedAtUtc { get; set; }
    public Guid? DeletedBy { get; set; }
}