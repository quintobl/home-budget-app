using API.DTOs;
using API.Entities;
using AutoMapper;

namespace API.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<AppUser, MemberDto>();
        CreateMap<Account, AccountDto>();

        // Credit Mappings
        CreateMap<Credit, CreditDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.DescriptionName, opt => opt.MapFrom(src => src.Description.Name)); // Mapping Description for Credit

        // Debit Mappings
        CreateMap<Debit, DebitDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.DescriptionName, opt => opt.MapFrom(src => src.Description.Name)); // Mapping Description for Debit
        CreateMap<DebitDto, Debit>();

        // Category Mappings
        CreateMap<Category, CategoryDto>();

        // Description Mappings
        CreateMap<Description, DescriptionDto>();
    }
}
