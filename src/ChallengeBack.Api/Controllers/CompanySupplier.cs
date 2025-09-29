using ChallengeBack.Application.Dto.CompanySupplier;
using Microsoft.AspNetCore.Mvc;


namespace ChallengeBack.Api.Controllers.CompanySupplier;

[Route("api/v1/company-supplier")]
[ApiController]
public class CompanySupplierController : ControllerBase {
    private readonly IAddCompanySupplierService _addCompanySupplierService;
    private readonly IDeleteCompanySupplierService _deleteCompanySupplierService;
    public CompanySupplierController(
        IAddCompanySupplierService addCompanySupplierService,
        IDeleteCompanySupplierService deleteCompanySupplierService) {
        _addCompanySupplierService = addCompanySupplierService;
        _deleteCompanySupplierService = deleteCompanySupplierService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] AddCompanySupplierDto createCompanySupplierDto, CancellationToken ct) {
        await _addCompanySupplierService.Execute(createCompanySupplierDto, ct);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct) {
        await _deleteCompanySupplierService.Execute(id, ct);
        return Ok();
    }
}
