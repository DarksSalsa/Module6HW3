using Catalog.Host.Data.Entities;

namespace Catalog.Host.Repositories.Interfaces
{
    public interface ICatalogBrandRepository
    {
        Task<CatalogBrand?> GetByIdAsync(int id);
        Task<int?> AddAsync(string brand);
        Task<int?> DeleteAsync(int id);
        Task<CatalogBrand?> UpdateAsync(int id, string property, string value);
    }
}
