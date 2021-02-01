using AutoMapper;
using AutoMapper.EquivalencyExpression;
using SimpleX.Data.Entities;
using SimpleX.Domain.Models;
using SimpleX.Domain.Models.Relations;
using SimpleX.Domain.Models.Shared;

namespace SimpleX.Domain.MapperProfiles
{
    public class AppointmentProfile : Profile
    {
        public AppointmentProfile()
        {
            CreateMap<Appointment, AppointmentDto>()
                .AfterMap((src, dest) => dest.Init())
                .ReverseMap()
                .EqualityComparison((x, y) => x.Id == y.Id);
        }
    }
}
