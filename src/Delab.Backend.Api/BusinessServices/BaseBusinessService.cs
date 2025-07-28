using Delab.Backend.Api.Notifications;
using Delab.Shared.Entities;
using FluentValidation;
using FluentValidation.Results;

namespace Delab.Backend.Api.BusinessServices;

public abstract class BaseBusinessService
{
    private readonly INotifier _notifier;

    protected BaseBusinessService(INotifier notifier)
    {
        _notifier = notifier;
    }

    protected void Notify(ValidationResult validationResult)
    {
        foreach (var error in validationResult.Errors)
        {
            Notify(error.ErrorMessage);
        }
    }

    protected void Notify(string message)
    {
        _notifier.Handle(new Notification(message));
    }

    protected bool ExecutarValidacao<TV, TE>(TV validation, TE entity) where TV : AbstractValidator<TE> where TE : BaseEntity
    {
        var validator = validation.Validate(entity);

        if (validator.IsValid) return true;

        Notify(validator);

        return false;
    }
}
