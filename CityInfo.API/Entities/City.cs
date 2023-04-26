using System.ComponentModel.DataAnnotations;

namespace CityInfo.API.Entities;

public class City
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = null!;
    
    [MaxLength(200)]
    public string? Description { get; set; }

    public ICollection<Attraction>? Attractions { get; set; }
}