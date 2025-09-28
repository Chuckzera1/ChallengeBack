using ChallengeBack.Application.Dto.Company;
using ChallengeBack.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChallengeBack.Api.Controllers.Company;
[Route("api/v1/company")]
[ApiController]
public class CreateCompanyController : ControllerBase {
    private readonly ICreateCompanyService _createCompanyService;

    public CreateCompanyController(ICreateCompanyService createCompanyService)
    {
        _createCompanyService = createCompanyService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public IActionResult Get()
    {
        return Ok("Company API is working!");
    }

    [HttpPost]
    [ProducesResponseType(typeof(Domain.Entities.Company), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateCompanyDto createCompanyDto, CancellationToken ct)
    {
        var company = await _createCompanyService.Execute(createCompanyDto, ct);
        return Ok(company);
    }
}