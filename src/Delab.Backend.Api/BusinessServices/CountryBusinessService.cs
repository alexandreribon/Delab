using Delab.Backend.Api.Notifications;
using Delab.Shared.Entities;
using Delab.Shared.Entities.Validations;
using Delab.Shared.Interfaces.BusinessServices;
using Delab.Shared.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Delab.Backend.Api.BusinessServices;

public class CountryBusinessService : BaseBusinessService, ICountryBusinessService
{
    private readonly ICountryRepository _countryRepository;

    public CountryBusinessService(ICountryRepository countryRepository, 
                                  INotifier notifier) : base(notifier)
    {
        _countryRepository = countryRepository;
    }

    public async Task<IEnumerable<Country>> GetAll()
    {
        var lstCountry = await _countryRepository.GetAll();
        return lstCountry;
    }

    public async Task<Country?> GetById(Guid id)
    {
        var country = await _countryRepository.GetById(id);
        return country;
    }

    public async Task<Country?> Create(Country country)
    {
        if (!ExecutarValidacao(new CountryValidation(), country)) return null;

        var foundCountry = _countryRepository.Consult(c => c.Name.Equals(country.Name)).FirstOrDefault();
        if (foundCountry != null)
        {
            Notify("Já existe um país com esse nome");
            return null;
        }

        var newCountry = await _countryRepository.Create(country);
        return newCountry;
    }

    public async Task<bool> Update(Country country)
    {
        if (!ExecutarValidacao(new CountryValidation(), country)) return false;

        var foundCountry = await _countryRepository.Consult(c => c.Name.Equals(country.Name)).FirstOrDefaultAsync();
        if (foundCountry != null && foundCountry.Id != country.Id)
        {
            Notify("Já existe um país com esse nome");
            return false;
        }

        var resp = await _countryRepository.Update(country);

        return resp;
    }

    public async Task<bool> Delete(Guid id)
    {
        var query = _countryRepository.Consult(c => c.Id.Equals(id));
        var country = await query.Include(c => c.States).FirstOrDefaultAsync();
        if (country!.States!.Count != 0)
        {
            Notify("O país possui estados cadastrados.");
            return false;
        }

        var resp = await _countryRepository.Delete(id);
        return resp;
    }
}
