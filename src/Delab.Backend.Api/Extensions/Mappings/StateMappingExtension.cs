using Delab.Backend.Api.Dtos;
using Delab.Shared.Entities;

namespace Delab.Backend.Api.Extensions.Mappings;

public static class StateMappingExtension
{
    public static StateDto? ToDto(this State state)
    {
        if (state == null) return null;

        return new StateDto
        {
            Id = state.Id,
            Name = state.Name,
            CountryId = state.CountryId,
            CountryName = state.Country!.Name,
            Cities = [.. state.Cities!.ToDto()!]
        };
    }

    public static IEnumerable<StateDto?> ToDto(this IEnumerable<State> states)
    {
        return states.Select(c => c.ToDto());
    }

    public static State? ToEntidade(this StateDto stateDto)
    {
        if (stateDto == null) return null;

        return new State
        {
            Id = stateDto.Id,
            Name = stateDto.Name,
            CountryId = stateDto.CountryId
        };
    }

    public static IEnumerable<State?> ToEntidade(this IEnumerable<StateDto> stateDtos)
    {
        return stateDtos.Select(dto => dto.ToEntidade());
    }
}
