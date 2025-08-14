using AutoMapper;
using AWP.DBContext.Models;
using AWP.Repository.DTO;

namespace AWP.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PuzzleMap, PuzzleMapDTO>().ReverseMap();
            // Add other mappings as needed
        }
    }
}
