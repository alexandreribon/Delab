using System.ComponentModel.DataAnnotations;

namespace Delab.Backend.Api.Dtos;

public class CityDto
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "O campo Nome é obrigatório")]
    [StringLength(150, ErrorMessage = "O campo Nome precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "O campo Estado é obrigatório")]
    public Guid StateId { get; set; }

    [ScaffoldColumn(false)]
    public string? StateName { get; set; }
}
