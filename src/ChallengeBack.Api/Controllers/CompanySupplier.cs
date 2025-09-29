using AutoMapper;
using ChallengeBack.Application.Dto.Base;
using ChallengeBack.Application.Dto.CompanySupplier;
using ChallengeBack.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChallengeBack.Api.Controllers.CompanySupplier;

[Route("api/v1/company-supplier")]
[ApiController]
public class CompanySupplierController : ControllerBase
{
    private readonly IAddCompanySupplierService _addCompanySupplierService;
    private readonly IDeleteCompanySupplierService _deleteCompanySupplierService;
    private readonly IGetAllCompanySuppliersService _getAllCompanySuppliersService;
    private readonly IMapper _mapper;

    public CompanySupplierController(
        IAddCompanySupplierService addCompanySupplierService,
        IDeleteCompanySupplierService deleteCompanySupplierService,
        IGetAllCompanySuppliersService getAllCompanySuppliersService,
        IMapper mapper)
    {
        _addCompanySupplierService = addCompanySupplierService;
        _deleteCompanySupplierService = deleteCompanySupplierService;
        _getAllCompanySuppliersService = getAllCompanySuppliersService;
        _mapper = mapper;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] AddCompanySupplierDto addCompanySupplierDto, CancellationToken ct)
    {
        await _addCompanySupplierService.Execute(addCompanySupplierDto, ct);
        return Created();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _deleteCompanySupplierService.Execute(id, ct);
        return Ok();
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResultDto<CompanySupplierListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllCompanySupplierFilterDto filter, CancellationToken ct)
    {
        var result = await _getAllCompanySuppliersService.Execute(filter, ct);
        var response = new PagedResultDto<CompanySupplierListDto>
        {
            Data = _mapper.Map<IEnumerable<CompanySupplierListDto>>(result.Data),
            TotalCount = result.TotalCount,
            Page = result.Page,
            Limit = result.Limit
        };
        return Ok(response);
    }
}