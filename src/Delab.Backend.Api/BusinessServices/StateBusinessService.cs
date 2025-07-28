using Delab.Backend.Api.Notifications;
using Delab.Shared.Entities;
using Delab.Shared.Entities.Validations;
using Delab.Shared.Interfaces.BusinessServices;
using Delab.Shared.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Delab.Backend.Api.BusinessServices;

public class StateBusinessService : BaseBusinessService, IStateBusinessService
{
    private readonly IStateRepository _stateRepository;

    public StateBusinessService(IStateRepository stateRepository,
                                INotifier notifier) : base(notifier)
    {
        _stateRepository = stateRepository;
    }

    public async Task<IEnumerable<State>> GetAll()
    {
        var lstState = await _stateRepository.GetAll();
        return lstState;
    }

    public async Task<State?> GetById(Guid id)
    {
        var state = await _stateRepository.GetById(id);
        return state;
    }

    public async Task<State?> Create(State state)
    {
        if (!ExecutarValidacao(new StateValidation(), state)) return null;

        var query = _stateRepository.Consult(c => c.Name.Equals(state.Name) &&
                                                  c.CountryId.Equals(state.CountryId));

        var foundState = await query.Include(s => s.Country).FirstOrDefaultAsync();
        if (foundState != null)
        {
            Notify($"Já existe um estado com esse nome para o país {foundState.Country!.Name}");
            return null;
        }

        var newState = await _stateRepository.Create(state);
        return newState;
    }

    public async Task<bool> Update(State state)
    {
        if (!ExecutarValidacao(new StateValidation(), state)) return false;

        var query = _stateRepository.Consult(c => c.Name.Equals(state.Name) &&
                                                  c.CountryId.Equals(state.CountryId));

        var foundState = await query.Include(s => s.Country).FirstOrDefaultAsync();
        if (foundState != null && foundState.Id != state.Id)
        {
            Notify($"Já existe um estado com esse nome para o país {foundState.Country!.Name}");
            return false;
        }

        var resp = await _stateRepository.Update(state);

        return resp;
    }

    public async Task<bool> Delete(Guid id)
    {
        var query = _stateRepository.Consult(c => c.Id.Equals(id));
        var state = await query.Include(c => c.Cities).FirstOrDefaultAsync();
        if (state!.Cities!.Count != 0)
        {
            Notify("O estado possui cidades cadastradas.");
            return false;
        }

        var resp = await _stateRepository.Delete(id);
        return resp;
    }
}
