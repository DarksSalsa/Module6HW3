using Catalog.Host.Data.Entities;

namespace Catalog.Host.Repositories.Interfaces
{
    public interface ICatalogTypeRepository
    {
        Task<CatalogType?> GetByIdAsync(int id);
        Task<int?> AddAsync(string type);
        Task<int?> DeleteAsync(int id);
        Task<CatalogType?> UpdateAsync(int id, string property, string value);
    }
}
