using System.Text.Json;
using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;

[Route("api/cities")]
[Authorize]
[ApiController]
public class CitiesController : ControllerBase
{
    private readonly IRepository _repository;
    private readonly IMapper _mapper;
    private const int maxPageSize = 20;

    public CitiesController(IRepository repository, IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CityWithoutAttractionsDto>>> Get(
        [FromQuery(Name = "Filter")] string? filterName,
        [FromQuery(Name = "Search")] string? searchQuery,
        [FromQuery(Name = "Page Number")] int pageNumber = 1,
        [FromQuery(Name = "Page Size")] int pageSize = 10)
    {
        if (pageSize > maxPageSize)
            pageSize = maxPageSize;

        var (cityEntities, paginationMetadata) =
            await _repository.GetCitiesAsync(filterName, searchQuery, pageNumber, pageSize);

        Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

        return Ok(_mapper.Map<IEnumerable<CityWithoutAttractionsDto>>(cityEntities));
    }

    [HttpGet("{cityId:int}")]
    public async Task<ActionResult> Get(int cityId, bool includeAttractions = false)
    {
        var city = await _repository.GetCityAsync(cityId, includeAttractions);

        if (city == null)
            return NotFound();

        return includeAttractions
            ? Ok(_mapper.Map<CityDto>(city))
            : Ok(_mapper.Map<CityWithoutAttractionsDto>(city));
    }
}