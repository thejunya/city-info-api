using System.ComponentModel.DataAnnotations;

namespace CityInfo.API.Entities;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Login { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    public string Password { get; set; } = null!;
    
    [Required]
    public bool IsAdmin { get; set; }
}