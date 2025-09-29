namespace ChallengeBack.Application.Dto.Company;

public class CreateCompanyDto
{
    public string Cnpj { get; set; }
    public string FantasyName { get; set; }
    public string ZipCode { get; set; }
}

public class UpdateCompanyDto {
    public string FantasyName { get; set; }
}