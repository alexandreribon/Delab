namespace Delab.Shared.Entities;

public class State : BaseEntity
{
    public string Name { get; set; } = null!;

    public Guid CountryId { get; set; }

    public Country? Country { get; set; }

    public ICollection<City>? Cities { get; set; }
}
