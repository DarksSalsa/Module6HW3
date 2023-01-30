using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories.Interfaces;

namespace Catalog.Host.Repositories;

public class CatalogItemRepository : ICatalogItemRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<CatalogItemRepository> _logger;

    public CatalogItemRepository(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<CatalogItemRepository> logger)
    {
        _dbContext = dbContextWrapper.DbContext;
        _logger = logger;
    }

    public async Task<PaginatedItems<CatalogItem>> GetByPageAsync(int pageIndex, int pageSize, int? brandFilter, int? typeFilter)
    {
        IQueryable<CatalogItem> query = _dbContext.CatalogItems;

        if (brandFilter.HasValue)
        {
            query = query.Where(w => w.CatalogBrandId == brandFilter.Value);
        }

        if (typeFilter.HasValue)
        {
            query = query.Where(w => w.CatalogTypeId == typeFilter.Value);
        }

        var totalItems = await query.LongCountAsync();

        var itemsOnPage = await query
            .Include(i => i.CatalogBrand)
            .Include(i => i.CatalogType)
            .OrderBy(c => c.Name)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedItems<CatalogItem>() { TotalCount = totalItems, Data = itemsOnPage };
    }

    public async Task<int?> Add(string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, string pictureFileName)
    {
        var item1 = new CatalogItem
        {
            CatalogBrandId = catalogBrandId,
            CatalogTypeId = catalogTypeId,
            Description = description,
            Name = name,
            PictureFileName = pictureFileName,
            Price = price
        };
        var item = await _dbContext.AddAsync(item1);

        await _dbContext.SaveChangesAsync();

        return item.Entity.Id;
    }

    public async Task<CatalogItem?> GetByIdAsync(int id)
    {
        var result = await _dbContext.CatalogItems.Include(i => i.CatalogBrand).Include(i => i.CatalogType).Where(i => i.Id == id).FirstOrDefaultAsync();
        return result;
    }

    public async Task<int?> DeleteAsync(int id)
    {
        var element = await GetByIdAsync(id);
        if (element != null)
        {
            _dbContext.CatalogItems.Remove(element);
            await _dbContext.SaveChangesAsync();
            return id;
        }

        return null;
    }

    public async Task<CatalogItem?> UpdateAsync(int id, string property, string value)
    {
        var result = await GetByIdAsync(id);

        if (result != null)
        {
            var changingValue = result.GetType().GetProperty(property);
            if (changingValue != null)
            {
                var propertyType = changingValue.PropertyType;
                var res = Convert.ChangeType(value, propertyType);
                changingValue.SetValue(result, res, null);
                _dbContext.CatalogItems.Update(result);
                await _dbContext.SaveChangesAsync();
                return result;
            }
        }

        return null;
    }

    public async Task<PaginatedItems<CatalogItem>> GetByBrandAsync(string brand, int pageIndex, int pageSize)
    {
        var totalItems = await _dbContext.CatalogItems
            .Where(w => w.CatalogBrand.Brand == brand)
            .CountAsync();

        var itemsOnPage = await _dbContext.CatalogItems
            .Include(i => i.CatalogBrand)
            .Include(i => i.CatalogType)
            .Where(w => w.CatalogBrand.Brand == brand)
            .OrderBy(c => c.Name)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedItems<CatalogItem>() { TotalCount = totalItems, Data = itemsOnPage };
    }

    public async Task<PaginatedItems<CatalogItem>> GetByTypeAsync(string type, int pageIndex, int pageSize)
    {
        var totalItems = await _dbContext.CatalogItems
            .Where(w => w.CatalogType.Type == type)
            .CountAsync();

        var itemsOnPage = await _dbContext.CatalogItems
            .Include(i => i.CatalogBrand)
            .Include(i => i.CatalogType)
            .Where(w => w.CatalogType.Type == type)
            .OrderBy(c => c.Name)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedItems<CatalogItem>() { TotalCount = totalItems, Data = itemsOnPage };
    }

    public async Task<PaginatedItems<CatalogBrand>> GetBrandsAsync()
    {
        var totalItems = await _dbContext.CatalogBrands
            .CountAsync();

        var itemsOnPage = await _dbContext.CatalogBrands.ToListAsync();

        return new PaginatedItems<CatalogBrand>() { TotalCount = totalItems, Data = itemsOnPage };
    }

    public async Task<PaginatedItems<CatalogType>> GetTypesAsync()
    {
        var totalItems = await _dbContext.CatalogTypes
            .CountAsync();

        var itemsOnPage = await _dbContext.CatalogTypes.ToListAsync();

        return new PaginatedItems<CatalogType>() { TotalCount = totalItems, Data = itemsOnPage };
    }
}