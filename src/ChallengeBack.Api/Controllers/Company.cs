using AutoMapper;
using ChallengeBack.Application.Dto.Base;
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
    private readonly IMapper _mapper;
    
    public CreateCompanyController(
        ICreateCompanyService createCompanyService, 
        IDeleteCompanyService deleteCompanyService,
        IGetAllCompaniesService getAllCompaniesService,
        IUpdateCompanyService updateCompanyService,
        IMapper mapper)
    {
        _createCompanyService = createCompanyService;
        _deleteCompanyService = deleteCompanyService;
        _getAllCompaniesService = getAllCompaniesService;
        _updateCompanyService = updateCompanyService;
        _mapper = mapper;
    }

    [HttpPost]
    [ProducesResponseType(typeof(CompanyResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateCompanyDto createCompanyDto, CancellationToken ct)
    {
        var company = await _createCompanyService.Execute(createCompanyDto, ct);
        var response = _mapper.Map<CompanyResponseDto>(company);
        return Ok(response);
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
    [ProducesResponseType(typeof(PagedResultDto<CompanyListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllCompanyFilterDto filter, CancellationToken ct)
    {
        var result = await _getAllCompaniesService.Execute(filter, ct);
        var response = new PagedResultDto<CompanyListDto>
        {
            Data = _mapper.Map<IEnumerable<CompanyListDto>>(result.Data),
            TotalCount = result.TotalCount,
            Page = result.Page,
            Limit = result.Limit
        };
        return Ok(response);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(CompanyResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCompanyDto updateCompanyDto, CancellationToken ct)
    {
        var company = await _updateCompanyService.Execute(id, updateCompanyDto, ct);
        var response = _mapper.Map<CompanyResponseDto>(company);
        return Ok(response);
    }
}