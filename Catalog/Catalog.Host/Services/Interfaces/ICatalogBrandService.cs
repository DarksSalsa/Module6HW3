using Catalog.Host.Models.Dtos;

namespace Catalog.Host.Services.Interfaces
{
    public interface ICatalogBrandService
    {
        Task<int?> AddAsync(string brand);
        Task<int?> DeleteAsync(int id);
        Task<CatalogBrandDto> UpdateAsync(int id, string property, string value);
    }
}
