using Catalog.Host.Models.Requests;
using Catalog.Host.Models.Response;
using Catalog.Host.Services.Interfaces;

namespace Catalog.Host.Controllers;

[ApiController]
[Route(ComponentDefaults.DefaultRoute)]
public class CatalogItemController : ControllerBase
{
    private readonly ILogger<CatalogItemController> _logger;
    private readonly ICatalogItemService _catalogItemService;

    public CatalogItemController(
        ILogger<CatalogItemController> logger,
        ICatalogItemService catalogItemService)
    {
        _logger = logger;
        _catalogItemService = catalogItemService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(UniversalAddResponse<int?>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Add(CreateProductRequest request)
    {
        var result = await _catalogItemService.AddAsync(request.Name, request.Description, request.Price, request.AvailableStock, request.CatalogBrandId, request.CatalogTypeId, request.PictureFileName);
        return Ok(new UniversalAddResponse<int?>() { Id = result });
    }

    [HttpPost]
    [ProducesResponseType(typeof(UniversalDeleteResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Delete(UniversalDeleteRequest request)
    {
        var result = await _catalogItemService.DeleteAsync(request.Id);
        if (result == null)
        {
            return BadRequest(new ErrorResponse() { ErrorMessage = "Undefined Id" });
        }

        return Ok(new UniversalDeleteResponse() { Message = "Success", Id = result });
    }

    [HttpPost]
    [ProducesResponseType(typeof(ItemUpdateResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Update(UniversalUpdateRequest request)
    {
        var result = await _catalogItemService.UpdateAsync(request.Id, request.Property, request.Value);
        if (result == null)
        {
            return BadRequest(new ErrorResponse() { ErrorMessage = "Undefined Id or Property" });
        }

        return Ok(new ItemUpdateResponse()
        {
            Id = result.Id,
            Name = result.Name,
            Description = result.Description,
            Price = result.Price,
            PictureUrl = result.PictureUrl,
            AvailableStock = result.AvailableStock,
            CatalogType = result.CatalogType,
            CatalogBrand = result.CatalogBrand
        });
    }
}