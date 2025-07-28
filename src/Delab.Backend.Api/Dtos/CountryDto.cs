using System.ComponentModel.DataAnnotations;

namespace Delab.Backend.Api.Dtos;

public class CountryDto
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "O campo País é obrigatório")]
    [StringLength(100, ErrorMessage = "O campo País precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
    public string Name { get; set; } = null!;

    public string? CodPhone { get; set; }

    public ICollection<StateDto>? States { get; set; }
}
