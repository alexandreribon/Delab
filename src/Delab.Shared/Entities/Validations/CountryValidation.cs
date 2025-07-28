using FluentValidation;

namespace Delab.Shared.Entities.Validations;

public class CountryValidation : AbstractValidator<Country>
{
    public CountryValidation()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("O campo Nome precisa ser fornecido")
            .Length(2, 100).WithMessage("O campo Nome precisa ter entre {MinLength} e {MaxLength} caracteres");

        RuleFor(c => c.CodPhone)
            .MaximumLength(5).WithMessage("O campo Código do Pais ultrapassou o tamanho máximo de 5 caracteres");

    }
}
