namespace Delab.Shared.Entities;

public class City : BaseEntity
{
    public string Name { get; set; } = null!;

    public Guid StateId { get; set; }

    public State? State { get; set; }
}
