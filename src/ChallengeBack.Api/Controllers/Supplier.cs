using ChallengeBack.Application.Dto.Supplier;
using ChallengeBack.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChallengeBack.Api.Controllers.Supplier;

[Route("api/v1/supplier")]
[ApiController]
public class CreateSupplierController : ControllerBase {
    private readonly ICreateSupplierService _createSupplierService;
    public CreateSupplierController(ICreateSupplierService createSupplierService)
    {
        _createSupplierService = createSupplierService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Domain.Entities.Supplier), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateSupplierDto createSupplierDto, CancellationToken ct)
    {
        var supplier = await _createSupplierService.Execute(createSupplierDto, ct);
        return Ok(supplier);
    }
}
