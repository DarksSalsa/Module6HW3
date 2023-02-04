using System.Threading;
using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Response;

namespace Catalog.UnitTests.Services
{
    public class CatalogTypeServiceTest
    {
        private readonly ICatalogTypeService _catalogService;

        private readonly Mock<ICatalogTypeRepository> _catalogTypeRepository;
        private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
        private readonly Mock<ILogger<CatalogTypeService>> _logger;
        private readonly Mock<IMapper> _mapper;

        private readonly CatalogType _catalogType = new CatalogType()
        {
            Type = "Test"
        };

        public CatalogTypeServiceTest()
        {
            _catalogTypeRepository = new Mock<ICatalogTypeRepository>();
            _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
            _logger = new Mock<ILogger<CatalogTypeService>>();
            _mapper = new Mock<IMapper>();

            var dbContextTransaction = new Mock<IDbContextTransaction>();
            _dbContextWrapper.Setup(s => s.BeginTransactionAsync(CancellationToken.None)).ReturnsAsync(dbContextTransaction.Object);

            _catalogService = new CatalogTypeService(_dbContextWrapper.Object, _logger.Object, _catalogTypeRepository.Object, _mapper.Object);
        }

        [Fact]
        public async Task AddAsync_Success()
        {
            // arrange
            var testId = 1;

            _catalogTypeRepository.Setup(s => s.AddAsync(It.IsAny<string>())).ReturnsAsync(testId);

            // act
            var result = await _catalogService.AddAsync(_catalogType.Type);

            // assert
            result.Should().Be(testId);
        }

        [Fact]
        public async Task AddAsync_Failed()
        {
            // arrange
            int? testId = null;

            _catalogTypeRepository.Setup(s => s.AddAsync(It.IsAny<string>())).ReturnsAsync(testId);

            // act
            var result = await _catalogService.AddAsync(_catalogType.Type);

            // assert
            result.Should().Be(testId);
        }

        [Fact]
        public async Task DeleteAsync_Success()
        {
            // arrange
            var testId = 1;
            _catalogTypeRepository.Setup(s => s.DeleteAsync(It.Is<int>(i => i == testId))).ReturnsAsync(testId);

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
            _catalogTypeRepository.Setup(s => s.DeleteAsync(It.Is<int>(i => i.Equals(testId)))).Returns((Func<UniversalDeleteResponse>)null!);

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

            var catalogTypeDtoSuccess = new CatalogTypeDto()
            {
                Type = "Test"
            };

            _catalogTypeRepository.Setup(s => s.UpdateAsync(
                It.Is<int>(i => i.Equals(testId)),
                It.Is<string>(i => i.Equals(testProperty)),
                It.Is<string>(i => i.Equals(testValue)))).ReturnsAsync(_catalogType);
            _mapper.Setup(s => s.Map<CatalogTypeDto>(It.Is<CatalogType>(i => i.Equals(_catalogType)))).Returns(catalogTypeDtoSuccess);

            // act
            var result = await _catalogService.UpdateAsync(testId, testProperty, testValue);

            // assert
            result.Should().NotBeNull();
            result?.Type.Should().Be(catalogTypeDtoSuccess.Type);
        }

        [Fact]
        public async Task UpdateAsync_Failed()
        {
            // arrange
            var testId = 1000000;
            var testProperty = "testProperty";
            var testValue = "testValue";

            _catalogTypeRepository.Setup(s => s.UpdateAsync(
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<string>())).Returns((Func<TypeUpdateResponse>)null!);

            // act
            var result = await _catalogService.UpdateAsync(testId, testProperty, testValue);

            // assert
            result.Should().BeNull();
        }
    }
}
