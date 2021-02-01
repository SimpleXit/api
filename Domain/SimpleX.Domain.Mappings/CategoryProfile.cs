using AutoMapper;
using AutoMapper.EquivalencyExpression;
using SimpleX.Data.Entities;
using SimpleX.Domain.Models;
using SimpleX.Domain.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleX.Domain.MapperProfiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CategoryTranslation, TranslationDto>()
                .AfterMap((src, dest) => dest.Init())
                .ReverseMap()
                .EqualityComparison((x, y) => x.Id == y.Id);


            CreateMap<Category, CategoryDto>()
                .AfterMap((src, dest) => dest.Init())
                .ReverseMap()
                .EqualityComparison((x, y) => x.Id == y.Id);
        }
    }
}
