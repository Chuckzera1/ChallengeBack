using ChallengeBack.Application.Dto.Base;

namespace ChallengeBack.Application.Dto.CompanySupplier;

public class GetAllCompanySupplierFilterDto : PaginationDto
{
    public int? CompanyId { get; set; }
    public int? SupplierId { get; set; }
}
