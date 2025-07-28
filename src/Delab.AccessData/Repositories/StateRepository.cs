using Delab.AccessData.Context;
using Delab.Shared.Entities;
using Delab.Shared.Interfaces.Repositories;

namespace Delab.AccessData.Repositories;

public class StateRepository : Repository<State>, IStateRepository
{
    public StateRepository(DBContext db) : base(db)
    {
    }
}
