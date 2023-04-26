using System.ComponentModel.DataAnnotations;

namespace CityInfo.API.Models;

public class AttractionCreateDto
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = null!;
    
    [MaxLength(200)]
    public string? Description { get; set; }
}