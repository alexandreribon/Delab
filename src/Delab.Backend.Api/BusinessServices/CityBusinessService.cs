using Delab.Backend.Api.Notifications;
using Delab.Shared.Entities;
using Delab.Shared.Entities.Validations;
using Delab.Shared.Interfaces.BusinessServices;
using Delab.Shared.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Delab.Backend.Api.BusinessServices;

public class CityBusinessService : BaseBusinessService, ICityBusinessService
{
    private readonly ICityRepository _cityRepository;

    public CityBusinessService(ICityRepository cityRepository,
                               INotifier notifier) : base(notifier)
    {
        _cityRepository = cityRepository;
    }

    public async Task<IEnumerable<City>> GetAll()
    {
        var lstCity = await _cityRepository.GetAll();
        return lstCity;
    }

    public async Task<City?> GetById(Guid id)
    {
        var city = await _cityRepository.GetById(id);
        return city;
    }

    public async Task<City?> Create(City city)
    {
        if (!ExecutarValidacao(new CityValidation(), city)) return null;

        var query = _cityRepository.Consult(c => c.Name.Equals(city.Name) &&
                                                 c.StateId.Equals(city.StateId));

        var cityState = await query.Include(c => c.State).FirstOrDefaultAsync();
        if (cityState != null)
        {
            Notify($"Já existe uma cidade com esse nome para o estado {cityState.State!.Name}");
            return null;
        }

        var newCity = await _cityRepository.Create(city);
        return newCity;
    }

    public async Task<bool> Update(City city)
    {
        if (!ExecutarValidacao(new CityValidation(), city)) return false;

        var query = _cityRepository.Consult(c => c.Name.Equals(city.Name) &&
                                                 c.StateId.Equals(city.StateId));

        var cityState = await query.Include(c => c.State).FirstOrDefaultAsync();
        if (cityState != null && cityState.Id != city.Id)
        {
            Notify($"Já existe uma cidade com esse nome para o estado {cityState.State!.Name}");
            return false;
        }

        var resp = await _cityRepository.Update(city);

        return resp;
    }

    public async Task<bool> Delete(Guid id)
    {
        var resp = await _cityRepository.Delete(id);
        return resp;
    }
}
