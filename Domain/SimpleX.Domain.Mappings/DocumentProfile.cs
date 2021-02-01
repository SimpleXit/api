using AutoMapper;
using AutoMapper.EquivalencyExpression;
using SimpleX.Data.Entities;
using SimpleX.Domain.Models;
using SimpleX.Domain.Models.Shared;

namespace SimpleX.Domain.MapperProfiles
{
    public class DocumentProfile : Profile
    {
        public DocumentProfile()
        {
            CreateMap<DocumentAssociateAddress, AddressDto>()
                .ReverseMap()
                .EqualityComparison((x, y) => x.Id == y.Id)
                .ForMember(dest => dest.DocumentAssociateID, opt => opt.Ignore());

            //CreateMap<CustomerNote, NoteDto>()
            //    .ReverseMap()
            //    .EqualityComparison((x, y) => x.Id == y.Id)
            //    .ForMember(dest => dest.CustomerID, opt => opt.Ignore())
            //    .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            //    .ForMember(dest => dest.CreatedOn, opt => opt.Ignore())
            //    .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
            //    .ForMember(dest => dest.UpdatedOn, opt => opt.Ignore())
            //    .ForMember(dest => dest.RowVersion, opt => opt.Ignore());

            //CreateMap<Document, DocumentDto>()
            //    .ReverseMap()
            //    .EqualityComparison((x, y) => x.Id == y.Id)
            //    .ForPath(dest => dest.ContactInfo.CreatedBy, opt => opt.Ignore())
            //    .ForPath(dest => dest.ContactInfo.CreatedOn, opt => opt.Ignore())
            //    .ForPath(dest => dest.ContactInfo.UpdatedBy, opt => opt.Ignore())
            //    .ForPath(dest => dest.ContactInfo.UpdatedOn, opt => opt.Ignore())
            //    .ForMember(dest => dest.ContactInfoID, opt => opt.Ignore())
            //    .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            //    .ForMember(dest => dest.CreatedOn, opt => opt.Ignore())
            //    .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
            //    .ForMember(dest => dest.UpdatedOn, opt => opt.Ignore());
        }
    }
}
