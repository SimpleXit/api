using AutoMapper;
using SimpleX.Data.Entities;
using SimpleX.Domain.Models;
using SimpleX.Domain.Models.App;
using SimpleX.Domain.Models.Shared;
using System.Linq;

namespace SimpleX.Domain.MapperProfiles
{
    public class AppProfile : Profile
    {
        public AppProfile()
        {
            CreateMap<AppMenuTranslation, TranslationDto>()
                .ReverseMap()
                .ForMember(dest => dest.MenuID, opt => opt.Ignore()); ;

            CreateMap<AppUser, AppUserDto>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(x => x.Roles.Select(r => r.Role)))
                .ReverseMap();


        }
    }
}
