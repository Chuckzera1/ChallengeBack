using AutoMapper;
using ChallengeBack.Application.Dto.Company;
using ChallengeBack.Application.Dto.Supplier;
using ChallengeBack.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChallengeBack.Api.Controllers.Supplier;

[Route("api/v1/supplier")]
[ApiController]
public class CreateSupplierController : ControllerBase {
    private readonly ICreateSupplierService _createSupplierService;
    private readonly IGetAllSuppliersWithFilterService _getAllSuppliersWithFilterService;
    private readonly IUpdateSupplierService _updateSupplierService;
    private readonly IDeleteSupplierService _deleteSupplierService;
    private readonly IMapper _mapper;
    
    public CreateSupplierController(
        ICreateSupplierService createSupplierService,
        IGetAllSuppliersWithFilterService getAllSuppliersWithFilterService,
        IUpdateSupplierService updateSupplierService,
        IDeleteSupplierService deleteSupplierService,
        IMapper mapper)
    {
        _createSupplierService = createSupplierService;
        _getAllSuppliersWithFilterService = getAllSuppliersWithFilterService;
        _updateSupplierService = updateSupplierService;
        _deleteSupplierService = deleteSupplierService;
        _mapper = mapper;
    }

    [HttpPost]
    [ProducesResponseType(typeof(SupplierResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateSupplierDto createSupplierDto, CancellationToken ct)
    {
        var supplier = await _createSupplierService.Execute(createSupplierDto, ct);
        var response = _mapper.Map<SupplierResponseDto>(supplier);
        return Ok(response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<SupplierListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllSupplierFilterDto filter, CancellationToken ct)
    {
        var suppliers = await _getAllSuppliersWithFilterService.Execute(filter, ct);
        var response = _mapper.Map<IEnumerable<SupplierListDto>>(suppliers);
        return Ok(response);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(SupplierResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateSupplierDto updateSupplierDto, CancellationToken ct)
    {
        var supplier = await _updateSupplierService.Execute(id, updateSupplierDto, ct);
        var response = _mapper.Map<SupplierResponseDto>(supplier);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _deleteSupplierService.Execute(id, ct);
        return Ok();
    }
}
