namespace Delab.Shared.Entities;

public class Country : BaseEntity
{
    public string Name { get; set; } = null!;
    public string? CodPhone { get; set; }

    public ICollection<State>? States { get; set; }
}
