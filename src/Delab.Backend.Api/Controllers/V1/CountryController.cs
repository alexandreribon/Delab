using Asp.Versioning;
using Delab.Backend.Api.Dtos;
using Delab.Backend.Api.Extensions.Mappings;
using Delab.Backend.Api.Notifications;
using Delab.Shared.Interfaces.BusinessServices;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Delab.Backend.Api.Controllers.V1;

[ApiVersion("1.0")]
[Route("api/v{version:ApiVersion}/country")]
public class CountryController : MainController
{
    private readonly ICountryBusinessService _countryBusinessService;

    public CountryController(ICountryBusinessService countryBusinessService,
                             INotifier notifier) : base(notifier)
    {
        _countryBusinessService = countryBusinessService;
    }

    [HttpGet]
    [Route("obter-todos")]
    [MapToApiVersion("1.0")]
    public async Task<ActionResult<IEnumerable<CountryDto>>> GetAll()
    {
        var lstCountry = await _countryBusinessService.GetAll();
        return CustomResponse(HttpStatusCode.OK, lstCountry);
    }

    [HttpGet]
    [Route("obter-por-id/{id:guid}")]
    [MapToApiVersion("1.0")]
    public async Task<ActionResult<CountryDto>> GetById(Guid id)
    {
        var country = await _countryBusinessService.GetById(id);

        if (country == null)
        {
            NotifyError("País não existe");
            return CustomResponse(HttpStatusCode.NotFound);
        }

        var countryDto = country.ToDto();

        return CustomResponse(HttpStatusCode.OK, countryDto);
    }

    [HttpPost]
    [Route("criar")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> Create([FromBody] CountryDto countryDto)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        var country = countryDto.ToEntidade();

        var newCountry = await _countryBusinessService.Create(country!);

        if (newCountry == null) return CustomResponse(HttpStatusCode.BadRequest);

        return CustomResponse(HttpStatusCode.Created, newCountry);
    }

    [HttpPut]
    [Route("editar/{id:guid}")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> Editar(Guid id, [FromBody] CountryDto countryDto)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        if (id != countryDto.Id)
        {
            NotifyError("Os ids informados não são iguais");
            return CustomResponse(HttpStatusCode.BadRequest);
        }

        var editCountry  = await _countryBusinessService.GetById(countryDto.Id);

        if (editCountry == null)
        {
            NotifyError("País não existe");
            return CustomResponse(HttpStatusCode.NotFound);
        }

        editCountry.Name = countryDto.Name;
        editCountry.CodPhone = countryDto.CodPhone;

        var resp = await _countryBusinessService.Update(editCountry);

        if (!resp) return CustomResponse(HttpStatusCode.BadRequest);

        return CustomResponse(HttpStatusCode.NoContent);
    }

    [HttpDelete]
    [Route("excluir/{id:guid}")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> Excluir(Guid id)
    {
        var country = await _countryBusinessService.GetById(id);

        if (country == null)
        {
            NotifyError("País não existe");
            return CustomResponse(HttpStatusCode.NotFound);
        }

        var resp = await _countryBusinessService.Delete(id);

        if (!resp) return CustomResponse(HttpStatusCode.BadRequest);

        return CustomResponse(HttpStatusCode.NoContent);
    }
}
