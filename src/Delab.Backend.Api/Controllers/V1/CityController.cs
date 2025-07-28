using Asp.Versioning;
using Delab.Backend.Api.Dtos;
using Delab.Backend.Api.Extensions.Mappings;
using Delab.Backend.Api.Notifications;
using Delab.Shared.Interfaces.BusinessServices;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Delab.Backend.Api.Controllers.V1;

[ApiVersion("1.0")]
[Route("api/v{version:ApiVersion}/city")]
public class CityController : MainController
{
    private readonly ICityBusinessService _cityBusinessService;

    public CityController(ICityBusinessService cityBusinessService,
                          INotifier notifier) : base(notifier)
    {
        _cityBusinessService = cityBusinessService;
    }

    [HttpGet]
    [Route("obter-todos")]
    [MapToApiVersion("1.0")]
    public async Task<ActionResult<IEnumerable<CountryDto>>> GetAll()
    {
        var lstCity = await _cityBusinessService.GetAll();
        return CustomResponse(HttpStatusCode.OK, lstCity);
    }

    [HttpGet]
    [Route("obter-por-id/{id:guid}")]
    [MapToApiVersion("1.0")]
    public async Task<ActionResult<StateDto>> GetById(Guid id)
    {
        var city = await _cityBusinessService.GetById(id);

        if (city == null)
        {
            NotifyError("Estado não existe");
            return CustomResponse(HttpStatusCode.NotFound);
        }

        var cityDto = city.ToDto();

        return CustomResponse(HttpStatusCode.OK, cityDto);
    }

    [HttpPost]
    [Route("criar")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> Create([FromBody] CityDto cityDto)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        var city = cityDto.ToEntidade();

        var newCity = await _cityBusinessService.Create(city!);

        if (newCity == null) return CustomResponse(HttpStatusCode.BadRequest);

        return CustomResponse(HttpStatusCode.Created, newCity);
    }

    [HttpPut]
    [Route("editar/{id:guid}")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> Editar(Guid id, [FromBody] CityDto cityDto)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        if (id != cityDto.Id)
        {
            NotifyError("Os ids informados não são iguais");
            return CustomResponse(HttpStatusCode.BadRequest);
        }

        var editCity = await _cityBusinessService.GetById(cityDto.Id);

        if (editCity == null)
        {
            NotifyError("Cidade não existe");
            return CustomResponse(HttpStatusCode.NotFound);
        }

        editCity.Name = cityDto.Name;
        editCity.StateId = cityDto.StateId;

        var resp = await _cityBusinessService.Update(editCity);

        if (!resp) return CustomResponse(HttpStatusCode.BadRequest);

        return CustomResponse(HttpStatusCode.NoContent);
    }

    [HttpDelete]
    [Route("excluir/{id:guid}")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> Excluir(Guid id)
    {
        var city = await _cityBusinessService.GetById(id);

        if (city == null)
        {
            NotifyError("Cidade não existe");
            return CustomResponse(HttpStatusCode.NotFound);
        }

        var resp = await _cityBusinessService.Delete(id);

        if (!resp) return CustomResponse(HttpStatusCode.BadRequest);

        return CustomResponse(HttpStatusCode.NoContent);
    }
}
