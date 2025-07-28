using Delab.Backend.Api.Notifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;

namespace Delab.Backend.Api.Controllers;

[ApiController]
public abstract class MainController : ControllerBase
{
    private readonly INotifier _notifier;

    public MainController(INotifier notifier)
    {
        _notifier = notifier;
    }

    protected ActionResult CustomResponse(ModelStateDictionary modelState)
    {
        var errors = modelState.Values.SelectMany(e => e.Errors);

        foreach (var err in errors)
        {
            var errMsg = err.Exception == null ? err.ErrorMessage : err.Exception.Message;
            NotifyError(errMsg);
        }

        return CustomResponse(HttpStatusCode.BadRequest);
    }

    protected ActionResult CustomResponse(HttpStatusCode statusCode, object? result = null)
    {
        switch (statusCode)
        {
            case HttpStatusCode.OK:
                return Ok(result);
            case HttpStatusCode.Created:
                return Created("", result);
            case HttpStatusCode.NoContent:
                return NoContent();
            case HttpStatusCode.NotFound:
            case HttpStatusCode.BadRequest:
            default:
                var messages = _notifier.GetNotifications().Select(n => n.Message!).ToList(); 
                if (statusCode == HttpStatusCode.NotFound) return NotFound(messages);
                return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
                {
                    { "Mensagens", messages.ToArray() }
                }));                            
        }
    }

    protected void NotifyError(string message)
    {
        _notifier.Handle(new Notification(message));
    }
}
