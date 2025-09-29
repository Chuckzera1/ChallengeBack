using AutoMapper;
using ChallengeBack.Application.Dto.Company;
using ChallengeBack.Application.Dto.CompanySupplier;
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
            .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company.FantasyName))
            .ForMember(dest => dest.CompanyCnpj, opt => opt.MapFrom(src => src.Company.Cnpj))
            .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier.Name))
            .ForMember(dest => dest.SupplierEmail, opt => opt.MapFrom(src => src.Supplier.Email))
            .ForMember(dest => dest.SupplierType, opt => opt.MapFrom(src => src.Supplier.Type.ToString()))
            .ForMember(dest => dest.SupplierCpf, opt => opt.MapFrom(src => src.Supplier.Cpf ?? string.Empty))
            .ForMember(dest => dest.SupplierCnpj, opt => opt.MapFrom(src => src.Supplier.Cnpj ?? string.Empty));
            
        CreateMap<CompanySupplier, CompanySupplierListDto>()
            .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company.FantasyName))
            .ForMember(dest => dest.CompanyCnpj, opt => opt.MapFrom(src => src.Company.Cnpj))
            .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier.Name))
            .ForMember(dest => dest.SupplierEmail, opt => opt.MapFrom(src => src.Supplier.Email))
            .ForMember(dest => dest.SupplierType, opt => opt.MapFrom(src => src.Supplier.Type.ToString()))
            .ForMember(dest => dest.SupplierCpf, opt => opt.MapFrom(src => src.Supplier.Cpf ?? string.Empty))
            .ForMember(dest => dest.SupplierCnpj, opt => opt.MapFrom(src => src.Supplier.Cnpj ?? string.Empty));
            
        CreateMap<CompanySupplier, SupplierCompanyListDto>()
            .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.Company));
        CreateMap<CompanySupplier, SupplierCompanyResponseDto>()
            .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.Company));
    }
}
