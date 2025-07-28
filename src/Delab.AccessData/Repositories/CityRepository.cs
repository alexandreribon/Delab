using Delab.AccessData.Context;
using Delab.Shared.Entities;
using Delab.Shared.Interfaces.Repositories;

namespace Delab.AccessData.Repositories;

public class CityRepository : Repository<City>, ICityRepository
{
    public CityRepository(DBContext db) : base(db)
    {
    }
}
