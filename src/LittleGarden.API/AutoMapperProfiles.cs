using AutoMapper;
using LittleGarden.API.DTO;
using LittleGarden.Core.Entities;

namespace LittleGarden.API
{
    internal class AutoMapperProfiles
    {
        internal class AutoMappingProfiles : Profile
        {
            public AutoMappingProfiles()
            {
                CreateMap<Seedling, SeedlingDto>().ReverseMap();
            }
        }
    }
}


