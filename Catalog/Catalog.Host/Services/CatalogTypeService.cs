using AutoMapper;
using Catalog.Host.Data;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;

namespace Catalog.Host.Services
{
    public class CatalogTypeService : BaseDataService<ApplicationDbContext>, ICatalogTypeService
    {
        private readonly ICatalogTypeRepository _catalogTypeRepository;
        private readonly IMapper _mapper;

        public CatalogTypeService(
            IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
            ILogger<BaseDataService<ApplicationDbContext>> logger,
            ICatalogTypeRepository catalogTypeRepository,
            IMapper mapper)
            : base(dbContextWrapper, logger)
        {
            _catalogTypeRepository = catalogTypeRepository;
            _mapper = mapper;
        }

        public async Task<int?> AddAsync(string brand)
        {
            return await ExecuteSafeAsync(async () => await _catalogTypeRepository.AddAsync(brand));
        }

        public async Task<int?> DeleteAsync(int id)
        {
            return await ExecuteSafeAsync(async () => await _catalogTypeRepository.DeleteAsync(id));
        }

        public Task<CatalogTypeDto> UpdateAsync(int id, string property, string value)
        {
            return ExecuteSafeAsync(async () =>
            {
                var result = await _catalogTypeRepository.UpdateAsync(id, property, value);
                return _mapper.Map<CatalogTypeDto>(result);
            });
        }
    }
}
