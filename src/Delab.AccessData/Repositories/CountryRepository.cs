using Delab.AccessData.Context;
using Delab.Shared.Entities;
using Delab.Shared.Interfaces.Repositories;

namespace Delab.AccessData.Repositories;

public class CountryRepository : Repository<Country>, ICountryRepository
{
    public CountryRepository(DBContext db) : base(db)
    {
    }
}
