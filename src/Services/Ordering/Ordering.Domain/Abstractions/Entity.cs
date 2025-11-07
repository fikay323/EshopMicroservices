namespace Ordering.Domain.Abstractions;

public abstract class Entity<T> : IEntity
{
    public T Id { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? LastUpdatedBy { get; set; }
    public DateTime? LastUpdated { get; set; }
}