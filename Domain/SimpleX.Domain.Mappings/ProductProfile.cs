using AutoMapper;
using AutoMapper.EquivalencyExpression;
using SimpleX.Data.Entities;
using SimpleX.Data.Entities.Assets;
using SimpleX.Domain.Models.Assets;
using SimpleX.Domain.Models.Shared;

namespace SimpleX.Domain.MapperProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductAttachment, AttachmentDto>()
                .AfterMap((src, dest) => dest.Init())
                .ReverseMap()
                .EqualityComparison((x, y) => x.Id == y.Id);

            CreateMap<ProductNote, NoteDto>()
                .AfterMap((src, dest) => dest.Init())
                .ReverseMap()
                .EqualityComparison((x, y) => x.Id == y.Id);


            CreateMap<Product, ProductDto>()
                .AfterMap((src, dest) => dest.Init())
                .ReverseMap()
                .EqualityComparison((x, y) => x.Id == y.Id);

        }
    }
}
