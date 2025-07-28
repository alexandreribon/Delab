using Delab.Shared.Entities;

namespace Delab.Shared.Interfaces.BusinessServices;

public interface ICityBusinessService
{
    Task<IEnumerable<City>> GetAll();
    Task<City?> GetById(Guid id);
    Task<City?> Create(City city);
    Task<bool> Update(City city);
    Task<bool> Delete(Guid id);
}
