using System.Threading;
using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Response;

namespace Catalog.UnitTests.Services
{
    public class CatalogBrandServiceTest
    {
        private readonly ICatalogBrandService _catalogService;

        private readonly Mock<ICatalogBrandRepository> _catalogBrandRepository;
        private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
        private readonly Mock<ILogger<CatalogBrandService>> _logger;
        private readonly Mock<IMapper> _mapper;

        private readonly CatalogBrand _catalogBrand = new CatalogBrand()
        {
            Brand = "Test"
        };

        public CatalogBrandServiceTest()
        {
            _catalogBrandRepository = new Mock<ICatalogBrandRepository>();
            _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
            _logger = new Mock<ILogger<CatalogBrandService>>();
            _mapper = new Mock<IMapper>();

            var dbContextTransaction = new Mock<IDbContextTransaction>();
            _dbContextWrapper.Setup(s => s.BeginTransactionAsync(CancellationToken.None)).ReturnsAsync(dbContextTransaction.Object);

            _catalogService = new CatalogBrandService(_dbContextWrapper.Object, _logger.Object, _catalogBrandRepository.Object, _mapper.Object);
        }

        [Fact]
        public async Task AddAsync_Success()
        {
            // arrange
            var testId = 1;

            _catalogBrandRepository.Setup(s => s.AddAsync(It.IsAny<string>())).ReturnsAsync(testId);

            // act
            var result = await _catalogService.AddAsync(_catalogBrand.Brand);

            // assert
            result.Should().Be(testId);
        }

        [Fact]
        public async Task AddAsync_Failed()
        {
            // arrange
            int? testId = null;

            _catalogBrandRepository.Setup(s => s.AddAsync(It.IsAny<string>())).ReturnsAsync(testId);

            // act
            var result = await _catalogService.AddAsync(_catalogBrand.Brand);

            // assert
            result.Should().Be(testId);
        }

        [Fact]
        public async Task DeleteAsync_Success()
        {
            // arrange
            var testId = 1;
            _catalogBrandRepository.Setup(s => s.DeleteAsync(It.Is<int>(i => i == testId))).ReturnsAsync(testId);

            // act
            var result = await _catalogService.DeleteAsync(testId);

            // assert
            result.Should().Be(testId);
        }

        [Fact]
        public async Task DeleteAsync_Failed()
        {
            // arrange
            var testId = 10000;
            _catalogBrandRepository.Setup(s => s.DeleteAsync(It.Is<int>(i => i.Equals(testId)))).Returns((Func<UniversalDeleteResponse>)null!);

            // act
            var result = await _catalogService.DeleteAsync(It.Is<int>(i => i.Equals(testId)));

            // assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task UpdateAsync_Success()
        {
            // arrange
            var testId = 1;
            var testProperty = "testProperty";
            var testValue = "testValue";

            var catalogBrandDtoSuccess = new CatalogBrandDto()
            {
                Brand = "Test"
            };

            _catalogBrandRepository.Setup(s => s.UpdateAsync(
                It.Is<int>(i => i.Equals(testId)),
                It.Is<string>(i => i.Equals(testProperty)),
                It.Is<string>(i => i.Equals(testValue)))).ReturnsAsync(_catalogBrand);
            _mapper.Setup(s => s.Map<CatalogBrandDto>(It.Is<CatalogBrand>(i => i.Equals(_catalogBrand)))).Returns(catalogBrandDtoSuccess);

            // act
            var result = await _catalogService.UpdateAsync(testId, testProperty, testValue);

            // assert
            result.Should().NotBeNull();
            result?.Brand.Should().Be(catalogBrandDtoSuccess.Brand);
        }

        [Fact]
        public async Task UpdateAsync_Failed()
        {
            // arrange
            var testId = 1000000;
            var testProperty = "testProperty";
            var testValue = "testValue";

            _catalogBrandRepository.Setup(s => s.UpdateAsync(
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<string>())).Returns((Func<BrandUpdateResponse>)null!);

            // act
            var result = await _catalogService.UpdateAsync(testId, testProperty, testValue);

            // assert
            result.Should().BeNull();
        }
    }
}
