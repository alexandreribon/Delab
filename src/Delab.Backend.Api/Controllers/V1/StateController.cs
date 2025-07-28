using Asp.Versioning;
using Delab.Backend.Api.Dtos;
using Delab.Backend.Api.Extensions.Mappings;
using Delab.Backend.Api.Notifications;
using Delab.Shared.Interfaces.BusinessServices;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Delab.Backend.Api.Controllers.V1;

[ApiVersion("1.0")]
[Route("api/v{version:ApiVersion}/state")]
public class StateController : MainController
{
    private readonly IStateBusinessService _stateBusinessService;

    public StateController(IStateBusinessService stateBusinessService,
                           INotifier notifier) : base(notifier)
    {
        _stateBusinessService = stateBusinessService;
    }

    [HttpGet]
    [Route("obter-todos")]
    [MapToApiVersion("1.0")]
    public async Task<ActionResult<IEnumerable<CountryDto>>> GetAll()
    {
        var lstState = await _stateBusinessService.GetAll();
        return CustomResponse(HttpStatusCode.OK, lstState);
    }

    [HttpGet]
    [Route("obter-por-id/{id:guid}")]
    [MapToApiVersion("1.0")]
    public async Task<ActionResult<StateDto>> GetById(Guid id)
    {
        var state = await _stateBusinessService.GetById(id);

        if (state == null)
        {
            NotifyError("Estado não existe");
            return CustomResponse(HttpStatusCode.NotFound);
        }

        var stateDto = state.ToDto();

        return CustomResponse(HttpStatusCode.OK, stateDto);
    }

    [HttpPost]
    [Route("criar")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> Create([FromBody] StateDto stateDto)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        var state = stateDto.ToEntidade();

        var newState = await _stateBusinessService.Create(state!);

        if (newState == null) return CustomResponse(HttpStatusCode.BadRequest);

        return CustomResponse(HttpStatusCode.Created, newState);
    }


    [HttpPut]
    [Route("editar/{id:guid}")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> Editar(Guid id, [FromBody] StateDto stateDto)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        if (id != stateDto.Id)
        {
            NotifyError("Os ids informados não são iguais");
            return CustomResponse(HttpStatusCode.BadRequest);
        }

        var editState = await _stateBusinessService.GetById(stateDto.Id);

        if (editState == null)
        {
            NotifyError("Estado não existe");
            return CustomResponse(HttpStatusCode.NotFound);
        }

        editState.Name = stateDto.Name;
        editState.CountryId = stateDto.CountryId;

        var resp = await _stateBusinessService.Update(editState);

        if (!resp) return CustomResponse(HttpStatusCode.BadRequest);

        return CustomResponse(HttpStatusCode.NoContent);
    }

    [HttpDelete]
    [Route("excluir/{id:guid}")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> Excluir(Guid id)
    {
        var state = await _stateBusinessService.GetById(id);

        if (state == null)
        {
            NotifyError("Estado não existe");
            return CustomResponse(HttpStatusCode.NotFound);
        }

        var resp = await _stateBusinessService.Delete(id);

        if (!resp) return CustomResponse(HttpStatusCode.BadRequest);

        return CustomResponse(HttpStatusCode.NoContent);
    }
}
