using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Services.Interfaces;

namespace Catalog.Host.Repositories
{
    public class CatalogBrandRepository : ICatalogBrandRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<CatalogItemRepository> _logger;

        public CatalogBrandRepository(
            IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
            ILogger<CatalogItemRepository> logger)
        {
            _dbContext = dbContextWrapper.DbContext;
            _logger = logger;
        }

        public async Task<int?> AddAsync(string brand)
        {
            var result = await _dbContext.CatalogBrands.AddAsync(new CatalogBrand() { Brand = brand });
            await _dbContext.SaveChangesAsync();
            return result.Entity.Id;
        }

        public async Task<CatalogBrand?> GetByIdAsync(int id)
        {
            var result = await _dbContext.CatalogBrands.Where(i => i.Id == id).FirstOrDefaultAsync();
            return result;
        }

        public async Task<int?> DeleteAsync(int id)
        {
            var element = await GetByIdAsync(id);
            if (element != null)
            {
                _dbContext.CatalogBrands.Remove(element);
                await _dbContext.SaveChangesAsync();
                return id;
            }

            return null;
        }

        public async Task<CatalogBrand?> UpdateAsync(int id, string property, string value)
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
                    _dbContext.CatalogBrands.Update(result);
                    await _dbContext.SaveChangesAsync();
                    return result;
                }
            }

            return null;
        }
    }
}
