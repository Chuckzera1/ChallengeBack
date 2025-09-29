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
    public CreateSupplierController(
        ICreateSupplierService createSupplierService,
        IGetAllSuppliersWithFilterService getAllSuppliersWithFilterService,
        IUpdateSupplierService updateSupplierService,
        IDeleteSupplierService deleteSupplierService)
    {
        _createSupplierService = createSupplierService;
        _getAllSuppliersWithFilterService = getAllSuppliersWithFilterService;
        _updateSupplierService = updateSupplierService;
        _deleteSupplierService = deleteSupplierService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Domain.Entities.Supplier), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateSupplierDto createSupplierDto, CancellationToken ct)
    {
        var supplier = await _createSupplierService.Execute(createSupplierDto, ct);
        return Ok(supplier);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Domain.Entities.Supplier>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllSupplierFilterDto filter, CancellationToken ct)
    {
        var suppliers = await _getAllSuppliersWithFilterService.Execute(filter, ct);
        return Ok(suppliers);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Domain.Entities.Supplier), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateSupplierDto updateSupplierDto, CancellationToken ct)
    {
        var supplier = await _updateSupplierService.Execute(id, updateSupplierDto, ct);
        return Ok(supplier);
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
