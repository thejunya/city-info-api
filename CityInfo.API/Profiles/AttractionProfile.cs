using AutoMapper;

namespace CityInfo.API.Profiles;

public class AttractionProfile : Profile
{
    public AttractionProfile()
    {
        CreateMap<Entities.Attraction, Models.AttractionDto>();
        CreateMap<Entities.Attraction, Models.AttractionUpdateDto>();

        CreateMap<Models.AttractionCreateDto, Entities.Attraction>();
        CreateMap<Models.AttractionUpdateDto, Entities.Attraction>();
    }
}