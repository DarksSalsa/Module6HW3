using Catalog.Host.Models.Dtos;

namespace Catalog.Host.Services.Interfaces
{
    public interface ICatalogTypeService
    {
        Task<int?> AddAsync(string brand);
        Task<int?> DeleteAsync(int id);
        Task<CatalogTypeDto> UpdateAsync(int id, string property, string value);
    }
}
