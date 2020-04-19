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
                CreateMap<Seedling, SeedlingDto>().ForMember(
                    d => d.Id, 
                    o => o.MapFrom(s=> s._id.ToString()));
            }
        }
    }
}