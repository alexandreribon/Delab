using System.ComponentModel.DataAnnotations;

namespace Delab.Backend.Api.Dtos;

public class StateDto
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "O campo Nome é obrigatório")]
    [StringLength(200, ErrorMessage = "O campo Nome precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "O campo País é obrigatório")]
    public Guid CountryId { get; set; }

    [ScaffoldColumn(false)]
    public string? CountryName { get; set; }

    public ICollection<CityDto>? Cities { get; set; }
}
