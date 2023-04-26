﻿namespace CityInfo.API.Models;

public class AttractionDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
    
    public string? Description { get; set; }
}