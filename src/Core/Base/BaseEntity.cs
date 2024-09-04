namespace Core.Base;

public abstract class BaseEntity : IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
}

public interface IEntity
{
    public Guid Id { get; set; }
}
