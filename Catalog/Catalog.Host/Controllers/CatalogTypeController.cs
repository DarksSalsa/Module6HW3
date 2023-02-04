using Catalog.Host.Models.Requests;
using Catalog.Host.Models.Response;
using Catalog.Host.Services.Interfaces;

namespace Catalog.Host.Controllers;

[ApiController]
[Route(ComponentDefaults.DefaultRoute)]
public class CatalogTypeController : ControllerBase
{
    private readonly ILogger<CatalogTypeController> _logger;
    private readonly ICatalogTypeService _catalogTypeService;

    public CatalogTypeController(ILogger<CatalogTypeController> logger, ICatalogTypeService catalogTypeService)
    {
        _logger = logger;
        _catalogTypeService = catalogTypeService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(UniversalAddResponse<int?>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Add(CreateTypeRequest request)
    {
        var result = await _catalogTypeService.AddAsync(request.Type);
        return Ok(new UniversalAddResponse<int?>() { Id = result });
    }

    [HttpPost]
    [ProducesResponseType(typeof(UniversalDeleteResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Delete(UniversalDeleteRequest request)
    {
        var result = await _catalogTypeService.DeleteAsync(request.Id);
        if (result == null)
        {
            return BadRequest(new ErrorResponse() { ErrorMessage = "Undefined Id" });
        }

        return Ok(new UniversalDeleteResponse() { Message = "Success", Id = result });
    }

    [HttpPost]
    [ProducesResponseType(typeof(TypeUpdateResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Update(UniversalUpdateRequest request)
    {
        var result = await _catalogTypeService.UpdateAsync(request.Id, request.Property, request.Value);
        if (result == null)
        {
            return BadRequest(new ErrorResponse() { ErrorMessage = "Undefined Id or Property" });
        }

        return Ok(new TypeUpdateResponse()
        {
            Id = result.Id,
            Type = result.Type
        });
    }
}