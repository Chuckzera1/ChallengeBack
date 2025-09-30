using AutoMapper;
using ChallengeBack.Domain.Entities;
using ChallengeBack.Application.Dto.Company;
using CompanySupplierListDtoEntity = ChallengeBack.Application.Dto.CompanySupplier.CompanySupplierListDto;
using SupplierCompanyListDtoEntity = ChallengeBack.Application.Dto.Supplier.SupplierCompanyListDto;
using ChallengeBack.Application.Dto.Supplier;

namespace ChallengeBack.Application.Mappers;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Company, CompanyResponseDto>();
        CreateMap<Company, CompanyListDto>();
        CreateMap<Company, CompanyBasicDto>();
        
        CreateMap<Supplier, SupplierResponseDto>();
        CreateMap<Supplier, SupplierListDto>();
        CreateMap<Supplier, SupplierBasicDto>();
        
        CreateMap<CompanySupplier, CompanySupplierListDtoEntity>()
            .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company.FantasyName))
            .ForMember(dest => dest.CompanyCnpj, opt => opt.MapFrom(src => src.Company.Cnpj))
            .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier.Name))
            .ForMember(dest => dest.SupplierEmail, opt => opt.MapFrom(src => src.Supplier.Email))
            .ForMember(dest => dest.SupplierType, opt => opt.MapFrom(src => src.Supplier.Type.ToString()))
            .ForMember(dest => dest.SupplierCpf, opt => opt.MapFrom(src => src.Supplier.Cpf ?? string.Empty))
            .ForMember(dest => dest.SupplierCnpj, opt => opt.MapFrom(src => src.Supplier.Cnpj ?? string.Empty));
            
        CreateMap<CompanySupplier, SupplierCompanyListDtoEntity>();
        CreateMap<CompanySupplier, SupplierCompanyResponseDto>();
            
        CreateMap<CompanySupplier, CompanySupplierListDto>();
    }
}
