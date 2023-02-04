using Catalog.Host.Data;
using Catalog.Host.Data.Entities;

namespace Catalog.Host.Repositories.Interfaces;

public interface ICatalogItemRepository
{
    Task<PaginatedItems<CatalogItem>> GetByPageAsync(int pageIndex, int pageSize, int? brandFilter, int? typeFilter);
    Task<int?> Add(string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, string pictureFileName);
    Task<int?> DeleteAsync(int id);
    Task<CatalogItem?> GetByIdAsync(int id);
    Task<CatalogItem?> UpdateAsync(int id, string property, string value);
    Task<PaginatedItems<CatalogItem>> GetByBrandAsync(string brand, int pageIndex, int pageSize);
    Task<PaginatedItems<CatalogItem>> GetByTypeAsync(string type, int pageIndex, int pageSize);
    Task<PaginatedItems<CatalogBrand>> GetBrandsAsync();
    Task<PaginatedItems<CatalogType>> GetTypesAsync();
}