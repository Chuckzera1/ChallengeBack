using ChallengeBack.Application.Dto.Base;

namespace ChallengeBack.Application.Dto.Company;

public class GetAllCompanyFilterDto : PaginationDto
{
    public string? Name { get; set; }
    public string? Cnpj { get; set; }
}
