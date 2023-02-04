using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Enums;
using Catalog.Host.Models.Response;

namespace Catalog.Host.Services.Interfaces;

public interface ICatalogService
{
    Task<PaginatedItemsResponse<CatalogItemDto>?> GetCatalogItemsAsync(int pageSize, int pageIndex, Dictionary<CatalogTypeFilter, int>? filters);
    Task<CatalogItemDto?> GetByIdAsync(int id);
    Task<PaginatedItemsResponse<CatalogItemDto>?> GetByBrandAsync(string brand, int pageIndex, int pageSize);
    Task<PaginatedItemsResponse<CatalogItemDto>?> GetByTypeAsync(string type, int pageIndex, int pageSize);
    Task<UniversalGetItemsResponse<CatalogBrandDto>?> GetBrandsAsync();
    Task<UniversalGetItemsResponse<CatalogTypeDto>?> GetTypesAsync();
}