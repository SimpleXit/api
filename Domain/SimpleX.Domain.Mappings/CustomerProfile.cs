using AutoMapper;
using AutoMapper.EquivalencyExpression;
using SimpleX.Data.Entities;
using SimpleX.Domain.Models;
using SimpleX.Domain.Models.Relations;
using SimpleX.Domain.Models.Shared;

namespace SimpleX.Domain.MapperProfiles
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<CustomerAddress, AddressDto>()
                .AfterMap((src, dest) => dest.Init())
                .ReverseMap()
                .EqualityComparison((x, y) => x.Id == y.Id);

            CreateMap<CustomerAttachment, AttachmentDto>()
                .AfterMap((src, dest) => dest.Init())
                .ReverseMap()
                .EqualityComparison((x, y) => x.Id == y.Id);

            CreateMap<CustomerCommunication, CommunicationDto>()
                .AfterMap((src, dest) => dest.Init())
                .ReverseMap()
                .EqualityComparison((x, y) => x.Id == y.Id);

            CreateMap<CustomerNote, NoteDto>()
                .AfterMap((src, dest) => dest.Init())
                .ReverseMap()
                .EqualityComparison((x, y) => x.Id == y.Id);

            CreateMap<CustomerPerson, PersonDto>()
                .AfterMap((src, dest) => dest.Init())
                .ReverseMap()
                .EqualityComparison((x, y) => x.Id == y.Id);


            CreateMap<Customer, CustomerDto>()
                .AfterMap((src, dest) => dest.Init())
                .ReverseMap()
                .EqualityComparison((x, y) => x.Id == y.Id);

        }
    }
}
