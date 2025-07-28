using FluentValidation;

namespace Delab.Shared.Entities.Validations;

public class CityValidation : AbstractValidator<City>
{
    public CityValidation()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("O campo Nome precisa ser fornecido")
            .Length(2, 150).WithMessage("O campo Nome precisa ter entre {MinLength} e {MaxLength} caracteres");
    }
}
