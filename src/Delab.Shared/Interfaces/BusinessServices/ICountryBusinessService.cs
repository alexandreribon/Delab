using Delab.Shared.Entities;

namespace Delab.Shared.Interfaces.BusinessServices;

public interface ICountryBusinessService
{
    Task<IEnumerable<Country>> GetAll();
    Task<Country?> GetById(Guid id);
    Task<Country?> Create(Country country);
    Task<bool> Update(Country country);
    Task<bool> Delete(Guid id);
}
