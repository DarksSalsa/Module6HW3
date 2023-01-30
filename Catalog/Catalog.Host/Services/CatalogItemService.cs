using Catalog.Host.Data;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;

namespace Catalog.Host.Services;

public class CatalogItemService : BaseDataService<ApplicationDbContext>, ICatalogItemService
{
    private readonly ICatalogItemRepository _catalogItemRepository;
    private readonly IMapper _mapper;

    public CatalogItemService(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<BaseDataService<ApplicationDbContext>> logger,
        ICatalogItemRepository catalogItemRepository,
        IMapper mapper)
        : base(dbContextWrapper, logger)
    {
        _catalogItemRepository = catalogItemRepository;
        _mapper = mapper;
    }

    public Task<int?> AddAsync(string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, string pictureFileName)
    {
        return ExecuteSafeAsync(() => _catalogItemRepository.Add(name, description, price, availableStock, catalogBrandId, catalogTypeId, pictureFileName));
    }

    public Task<int?> DeleteAsync(int id)
    {
        return ExecuteSafeAsync(async () =>
        {
            var result = await _catalogItemRepository.DeleteAsync(id);
            return result;
        });
    }

    public Task<CatalogItemDto> UpdateAsync(int id, string property, string value)
    {
        return ExecuteSafeAsync(async () =>
        {
            var result = await _catalogItemRepository.UpdateAsync(id, property, value);
            return _mapper.Map<CatalogItemDto>(result);
        });
    }
}