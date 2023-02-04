using AutoMapper;
using Catalog.Host.Data;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;

namespace Catalog.Host.Services
{
    public class CatalogBrandService : BaseDataService<ApplicationDbContext>, ICatalogBrandService
    {
        private readonly ICatalogBrandRepository _catalogBrandRepository;
        private readonly IMapper _mapper;

        public CatalogBrandService(
            IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
            ILogger<BaseDataService<ApplicationDbContext>> logger,
            ICatalogBrandRepository catalogBrandRepository,
            IMapper mapper)
            : base(dbContextWrapper, logger)
        {
            _catalogBrandRepository = catalogBrandRepository;
            _mapper = mapper;
        }

        public async Task<int?> AddAsync(string brand)
        {
            return await ExecuteSafeAsync(async () => await _catalogBrandRepository.AddAsync(brand));
        }

        public async Task<int?> DeleteAsync(int id)
        {
            return await ExecuteSafeAsync(async () => await _catalogBrandRepository.DeleteAsync(id));
        }

        public Task<CatalogBrandDto> UpdateAsync(int id, string property, string value)
        {
            return ExecuteSafeAsync(async () =>
            {
                var result = await _catalogBrandRepository.UpdateAsync(id, property, value);
                return _mapper.Map<CatalogBrandDto>(result);
            });
        }
    }
}
