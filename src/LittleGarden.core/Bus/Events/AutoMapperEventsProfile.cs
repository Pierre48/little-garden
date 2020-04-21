using AutoMapper;
using LittleGarden.Core.Entities;

namespace LittleGarden.Core.Bus.Events
{
    public class AutoMapperEventsProfile
    {
        internal class AutoMappingProfiles : Profile
        {
            public AutoMappingProfiles()
            {
                CreateMap<Seedling, SeedlingEvent>().ReverseMap();
                CreateMap<Image, ImageEvent>().ReverseMap();
                CreateMap<Interest, InterestEvent>().ReverseMap();
            }
        }
    }
}