using ChallengeBack.Application.Dto.Base;

namespace ChallengeBack.Application.Dto.Supplier;

public class GetAllSupplierFilterDto : PaginationDto
{
    public string? Name { get; set; }
    public string? Cpf { get; set; }
    public string? Cnpj { get; set; }
}
