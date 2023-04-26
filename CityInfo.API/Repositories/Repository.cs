using CityInfo.API.Contexts;
using CityInfo.API.Entities;
using CityInfo.API.Services;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Repositories;

public class Repository : IRepository
{
    private readonly CityInfoContext _context;

    public Repository(CityInfoContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<City>> GetCitiesAsync()
    {
        return await _context.Cities.ToListAsync();
    }

    public async Task<(IEnumerable<City>, PaginationMetadata)> GetCitiesAsync(
        string? filterName,
        string? searchQuery,
        int pageNumber,
        int pageSize)
    {
        var collection = _context.Cities as IQueryable<City>;

        if (!string.IsNullOrWhiteSpace(filterName))
        {
            filterName = filterName.Trim();
            collection = collection.Where(c => c.Name == filterName);
        }

        if (!string.IsNullOrWhiteSpace(searchQuery))
        {
            searchQuery = searchQuery.Trim();
            collection = collection.Where(c =>
                c.Name.Contains(searchQuery) ||
                c.Description != null &&
                c.Description.Contains(searchQuery));
        }

        var totalItemCount = await collection.CountAsync();

        var paginationMetadataToReturn = new PaginationMetadata(totalItemCount, pageSize, pageNumber);

        var collectionToReturn = await collection
            .OrderBy(c => c.Name)
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .ToListAsync();

        return (collectionToReturn, paginationMetadataToReturn);
    }

    public async Task<City?> GetCityAsync(int cityId, bool includeAttractions)
    {
        if (includeAttractions)
            return await _context.Cities
                .Include(c => c.Attractions)
                .Where(c => c.Id == cityId)
                .FirstOrDefaultAsync();

        return await _context.Cities
            .Where(c => c.Id == cityId)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> CityExistsAsync(int cityId)
    {
        return await _context.Cities.AnyAsync(c => c.Id == cityId);
    }

    public async Task<IEnumerable<Attraction>> GetAttractionsAsync(int cityId)
    {
        return await _context.Attractions
            .Where(a => a.CityId == cityId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Attraction>> GetAttractionsOfCityAsync(int cityId)
    {
        return await _context.Attractions
            .Where(a => a.CityId == cityId)
            .ToListAsync();
    }

    public async Task<Attraction?> GetAttractionAsync(int cityId, int attractionId)
    {
        return await _context.Attractions
            .Where(a => a.CityId == cityId && a.Id == attractionId)
            .FirstOrDefaultAsync();
    }

    public async Task AddAttractionAsync(int cityId, Attraction attraction)
    {
        var cityEntity = await GetCityAsync(cityId, false);

        if (cityEntity != null)
        {
            if (cityEntity.Attractions == null)
                cityEntity.Attractions = new List<Attraction>() { attraction };
            else
                cityEntity.Attractions.Add(attraction);
        }
    }

    public void DeleteAttraction(Attraction attraction)
    {
        _context.Attractions.Remove(attraction);
    }

    public async Task<User?> GetUser(string? login, string? password)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Login == login && u.Password == password);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}