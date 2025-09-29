using ChallengeBack.Application.Dto.Company;
using ChallengeBack.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChallengeBack.Api.Controllers.Company;
[Route("api/v1/company")]
[ApiController]
public class CreateCompanyController : ControllerBase {
    private readonly ICreateCompanyService _createCompanyService;
    private readonly IDeleteCompanyService _deleteCompanyService;
    private readonly IGetAllCompaniesService _getAllCompaniesService;
    private readonly IUpdateCompanyService _updateCompanyService;
    public CreateCompanyController(
        ICreateCompanyService createCompanyService, 
        IDeleteCompanyService deleteCompanyService,
        IGetAllCompaniesService getAllCompaniesService,
        IUpdateCompanyService updateCompanyService)
    {
        _createCompanyService = createCompanyService;
        _deleteCompanyService = deleteCompanyService;
        _getAllCompaniesService = getAllCompaniesService;
        _updateCompanyService = updateCompanyService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Domain.Entities.Company), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateCompanyDto createCompanyDto, CancellationToken ct)
    {
        var company = await _createCompanyService.Execute(createCompanyDto, ct);
        return Ok(company);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _deleteCompanyService.Execute(id, ct);
        return Ok();
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Domain.Entities.Company>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var companies = await _getAllCompaniesService.Execute(ct);
        return Ok(companies);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Domain.Entities.Company), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCompanyDto updateCompanyDto, CancellationToken ct)
    {
        var company = await _updateCompanyService.Execute(id, updateCompanyDto, ct);
        return Ok(company);
    }
}