using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Models;
using CityInfo.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;

[Route("api/cities/{cityId:int}/attractions")]
[Authorize]
[ApiController]
public class AttractionsController : ControllerBase
{
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public AttractionsController(IRepository repository, IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AttractionDto>>> Get(int cityId)
    {
        if (!await _repository.CityExistsAsync(cityId))
            return NotFound();

        var attractionEntities = await _repository.GetAttractionsAsync(cityId);

        return Ok(_mapper.Map<IEnumerable<AttractionDto>>(attractionEntities));
    }

    [HttpGet("{attractionId:int}", Name = "GetAttraction")]
    public async Task<ActionResult<AttractionDto>> Get(int cityId, int attractionId)
    {
        if (!await _repository.CityExistsAsync(cityId))
            return NotFound();

        var attractionEntity = await _repository.GetAttractionAsync(cityId, attractionId);

        if (attractionEntity == null)
            return NotFound();

        return Ok(_mapper.Map<AttractionDto>(attractionEntity));
    }

    [HttpPost]
    public async Task<ActionResult<AttractionDto>> Post(int cityId, AttractionCreateDto attractionCreate)
    {
        if (!await _repository.CityExistsAsync(cityId))
            return NotFound();

        var mappedAttractionEntity = _mapper.Map<Attraction>(attractionCreate);

        await _repository.AddAttractionAsync(cityId, mappedAttractionEntity);
        await _repository.SaveChangesAsync();

        var attractionToReturn = _mapper.Map<AttractionDto>(mappedAttractionEntity);

        return CreatedAtRoute(
            "GetAttraction",
            new
            {
                cityId,
                attractionId = attractionToReturn.Id
            },
            attractionToReturn);
    }

    [HttpPut("{attractionId:int}")]
    public async Task<ActionResult> Put(int cityId, int attractionId, AttractionUpdateDto attractionUpdate)
    {
        if (!await _repository.CityExistsAsync(cityId))
            return NotFound();

        var attractionEntity = await _repository.GetAttractionAsync(cityId, attractionId);

        if (attractionEntity == null)
            return NotFound();

        _mapper.Map(attractionUpdate, attractionEntity);
        await _repository.SaveChangesAsync();

        return NoContent();
    }

    [HttpPatch("{attractionId:int}")]
    public async Task<ActionResult> Patch(int cityId, int attractionId, JsonPatchDocument<AttractionUpdateDto> patch)
    {
        if (!await _repository.CityExistsAsync(cityId))
            return NotFound();

        var attractionEntity = await _repository.GetAttractionAsync(cityId, attractionId);

        if (attractionEntity == null)
            return NotFound();

        var attractionToPatch = _mapper.Map<AttractionUpdateDto>(attractionEntity);

        patch.ApplyTo(attractionToPatch);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!TryValidateModel(attractionToPatch))
            return BadRequest(ModelState);

        _mapper.Map(attractionToPatch, attractionEntity);
        await _repository.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{attractionId:int}")]
    public async Task<ActionResult> Delete(int cityId, int attractionId)
    {
        if (!await _repository.CityExistsAsync(cityId))
            return NotFound();

        var attractionEntity = await _repository.GetAttractionAsync(cityId, attractionId);

        if (attractionEntity == null)
            return NotFound();

        _repository.DeleteAttraction(attractionEntity);
        await _repository.SaveChangesAsync();

        return NoContent();
    }
}