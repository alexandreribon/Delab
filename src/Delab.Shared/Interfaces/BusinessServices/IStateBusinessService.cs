using Delab.Shared.Entities;

namespace Delab.Shared.Interfaces.BusinessServices;

public interface IStateBusinessService
{
    Task<IEnumerable<State>> GetAll();
    Task<State?> GetById(Guid id);
    Task<State?> Create(State state);
    Task<bool> Update(State state);
    Task<bool> Delete(Guid id);
}
