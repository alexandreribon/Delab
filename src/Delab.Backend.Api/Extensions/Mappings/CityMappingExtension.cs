using Delab.Backend.Api.Dtos;
using Delab.Shared.Entities;

namespace Delab.Backend.Api.Extensions.Mappings;

public static class CityMappingExtension
{
    public static CityDto? ToDto(this City city)
    {
        if (city == null) return null;

        return new CityDto
        {
            Id = city.Id,
            Name = city.Name,
            StateId = city.StateId,
            StateName = city.State!.Name
        };
    }

    public static IEnumerable<CityDto?> ToDto(this IEnumerable<City> cities)
    {
        return cities.Select(c => c.ToDto());
    }

    public static City? ToEntidade(this CityDto cityDto)
    {
        if (cityDto == null) return null;

        return new City
        {
            Id = cityDto.Id,
            Name = cityDto.Name,
            StateId = cityDto.StateId
        };
    }

    public static IEnumerable<City?> ToEntidade(this IEnumerable<CityDto> cityDtos)
    {
        return cityDtos.Select(dto => dto.ToEntidade());
    }
}
