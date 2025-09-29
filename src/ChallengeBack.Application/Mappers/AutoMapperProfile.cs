using AutoMapper;
using ChallengeBack.Application.Dto.Company;
using ChallengeBack.Application.Dto.Supplier;
using ChallengeBack.Domain.Entities;

namespace ChallengeBack.Application.Mappers;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // Company mappings
        CreateMap<Company, CompanyResponseDto>()
            .ForMember(dest => dest.CompanySuppliers, opt => opt.MapFrom(src => src.CompanySuppliers));
        CreateMap<Company, CompanyListDto>()
            .ForMember(dest => dest.CompanySuppliers, opt => opt.MapFrom(src => src.CompanySuppliers));
        CreateMap<Company, CompanyBasicDto>();
        
        // Supplier mappings
        CreateMap<Supplier, SupplierResponseDto>()
            .ForMember(dest => dest.CompanySuppliers, opt => opt.MapFrom(src => src.CompanySuppliers));
        CreateMap<Supplier, SupplierListDto>()
            .ForMember(dest => dest.CompanySuppliers, opt => opt.MapFrom(src => src.CompanySuppliers));
        CreateMap<Supplier, SupplierBasicDto>();
        
        // CompanySupplier mappings
        CreateMap<CompanySupplier, CompanySupplierResponseDto>()
            .ForMember(dest => dest.Supplier, opt => opt.MapFrom(src => src.Supplier));
        CreateMap<CompanySupplier, CompanySupplierListDto>()
            .ForMember(dest => dest.Supplier, opt => opt.MapFrom(src => src.Supplier));
        CreateMap<CompanySupplier, SupplierCompanyListDto>()
            .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.Company));
        CreateMap<CompanySupplier, SupplierCompanyResponseDto>()
            .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.Company));
    }
}
