using Delab.Backend.Api.Dtos;
using Delab.Shared.Entities;

namespace Delab.Backend.Api.Extensions.Mappings;

public static class CountryMappingExtension
{
    public static CountryDto? ToDto(this Country country)
    {
        if (country == null) return null;

        return new CountryDto
        {
            Id = country.Id,
            Name = country.Name,
            CodPhone = country.CodPhone,
            States = [.. country.States!.ToDto()!]
        };
    }

    public static IEnumerable<CountryDto?> ToDto(this IEnumerable<Country> countries)
    {
        return countries.Select(c => c.ToDto());
    }

    public static Country? ToEntidade(this CountryDto countryDto)
    {
        if (countryDto == null) return null;

        return new Country
        {
            Id = countryDto.Id,
            Name = countryDto.Name,
            CodPhone = countryDto.CodPhone
        };
    }

    public static IEnumerable<Country?> ToEntidade(this IEnumerable<CountryDto> countryDtos)
    {
        return countryDtos.Select(dto => dto.ToEntidade());
    }
}
