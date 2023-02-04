using Catalog.Host.Configurations;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Enums;
using Catalog.Host.Models.Requests;
using Catalog.Host.Models.Response;
using Catalog.Host.Services.Interfaces;

namespace Catalog.Host.Controllers;

[ApiController]
[Route(ComponentDefaults.DefaultRoute)]
public class CatalogBffController : ControllerBase
{
    private readonly ILogger<CatalogBffController> _logger;
    private readonly ICatalogService _catalogService;
    private readonly IOptions<CatalogConfig> _config;

    public CatalogBffController(
        ILogger<CatalogBffController> logger,
        ICatalogService catalogService,
        IOptions<CatalogConfig> config)
    {
        _logger = logger;
        _catalogService = catalogService;
        _config = config;
    }

    [HttpPost]
    [ProducesResponseType(typeof(PaginatedItemsResponse<CatalogItemDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Items(PaginatedItemsRequest<CatalogTypeFilter> request)
    {
        var result = await _catalogService.GetCatalogItemsAsync(request.PageSize, request.PageIndex, request.Filters);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(GetItemByIdResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetById(UniversalGetByIdRequest request)
    {
        var result = await _catalogService.GetByIdAsync(request.Id);
        if (result == null)
        {
            return BadRequest(new ErrorResponse() { ErrorMessage = "Undefined Id" });
        }
        else
        {
            return Ok(new GetItemByIdResponse()
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

    [HttpPost]
    [ProducesResponseType(typeof(PaginatedItemsResponse<CatalogItemDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetByBrand(PaginatedItemsByBrandRequest request)
    {
        var result = await _catalogService.GetByBrandAsync(request.Brand, request.PageIndex, request.PageSize);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(PaginatedItemsResponse<CatalogItemDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetByType(PaginatedItemsByTypeRequest request)
    {
        var result = await _catalogService.GetByTypeAsync(request.Type, request.PageIndex, request.PageSize);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(UniversalGetItemsResponse<CatalogBrandDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Brands()
    {
        var result = await _catalogService.GetBrandsAsync();
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(UniversalGetItemsResponse<CatalogTypeDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Types()
    {
        var result = await _catalogService.GetTypesAsync();
        return Ok(result);
    }
}