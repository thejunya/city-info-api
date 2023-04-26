using CityInfo.API.Entities;
using CityInfo.API.Services;

namespace CityInfo.API.Repositories;

public interface IRepository
{
    Task<IEnumerable<City>> GetCitiesAsync();

    Task<(IEnumerable<City>, PaginationMetadata)> GetCitiesAsync(
        string? filterName,
        string? searchQuery,
        int pageNumber,
        int pageSize);

    Task<City?> GetCityAsync(int cityId, bool includeAttractions);
    
    Task<bool> CityExistsAsync(int cityId);

    Task<IEnumerable<Attraction>> GetAttractionsAsync(int cityId);

    Task<Attraction?> GetAttractionAsync(int cityId, int attractionId);

    Task AddAttractionAsync(int cityId, Attraction attraction);

    void DeleteAttraction(Attraction attraction);

    Task<User?> GetUser(string? login, string? password);

    Task<int> SaveChangesAsync();
}