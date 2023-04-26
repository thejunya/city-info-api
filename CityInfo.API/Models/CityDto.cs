namespace CityInfo.API.Models;

public class CityDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public ICollection<AttractionDto>? Attractions { get; set; }

    public int NumberOfAttractions => Attractions?.Count ?? 0;
}