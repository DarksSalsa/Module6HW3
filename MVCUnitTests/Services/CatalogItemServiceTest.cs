using System.Threading;
using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Response;
using Moq;

namespace Catalog.UnitTests.Services;

public class CatalogItemServiceTest
{
    private readonly ICatalogItemService _catalogService;

    private readonly Mock<ICatalogItemRepository> _catalogItemRepository;
    private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
    private readonly Mock<ILogger<CatalogBrandService>> _logger;
    private readonly Mock<IMapper> _mapper;

    private readonly CatalogItem _testItem = new CatalogItem()
    {
        Name = "Name",
        Description = "Description",
        Price = 1000,
        AvailableStock = 100,
        CatalogBrandId = 1,
        CatalogTypeId = 1,
        PictureFileName = "1.png"
    };

    public CatalogItemServiceTest()
    {
        _catalogItemRepository = new Mock<ICatalogItemRepository>();
        _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
        _logger = new Mock<ILogger<CatalogBrandService>>();
        _mapper = new Mock<IMapper>();

        var dbContextTransaction = new Mock<IDbContextTransaction>();
        _dbContextWrapper.Setup(s => s.BeginTransactionAsync(CancellationToken.None)).ReturnsAsync(dbContextTransaction.Object);

        _catalogService = new CatalogItemService(_dbContextWrapper.Object, _logger.Object, _catalogItemRepository.Object, _mapper.Object);
    }

    [Fact]
    public async Task AddAsync_Success()
    {
        // arrange
        var testResult = 1;

        _catalogItemRepository.Setup(s => s.Add(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<decimal>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogService.AddAsync(_testItem.Name, _testItem.Description, _testItem.Price, _testItem.AvailableStock, _testItem.CatalogBrandId, _testItem.CatalogTypeId, _testItem.PictureFileName);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task AddAsync_Failed()
    {
        // arrange
        int? testResult = null;

        _catalogItemRepository.Setup(s => s.Add(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<decimal>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogService.AddAsync(_testItem.Name, _testItem.Description, _testItem.Price, _testItem.AvailableStock, _testItem.CatalogBrandId, _testItem.CatalogTypeId, _testItem.PictureFileName);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task DeleteAsync_Success()
    {
        // arrange
        var testId = 1;
        _catalogItemRepository.Setup(s => s.DeleteAsync(It.Is<int>(i => i == testId))).ReturnsAsync(testId);

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
        _catalogItemRepository.Setup(s => s.DeleteAsync(It.Is<int>(i => i.Equals(testId)))).Returns((Func<UniversalDeleteResponse>)null!);

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
        var catalogItemSuccess = new CatalogItem()
        {
            Name = "Test"
        };

        var catalogItemDtoSuccess = new CatalogItemDto()
        {
            Name = "Test"
        };

        _catalogItemRepository.Setup(s => s.UpdateAsync(
            It.Is<int>(i => i.Equals(testId)),
            It.Is<string>(i => i.Equals(testProperty)),
            It.Is<string>(i => i.Equals(testValue)))).ReturnsAsync(catalogItemSuccess);
        _mapper.Setup(s => s.Map<CatalogItemDto>(It.Is<CatalogItem>(i => i.Equals(catalogItemSuccess)))).Returns(catalogItemDtoSuccess);

        // act
        var result = await _catalogService.UpdateAsync(testId, testProperty, testValue);

        // assert
        result.Should().NotBeNull();
        result?.Name.Should().Be(catalogItemDtoSuccess.Name);
    }

    [Fact]
    public async Task UpdateAsync_Failed()
    {
        // arrange
        var testId = 1000000;
        var testProperty = "testProperty";
        var testValue = "testValue";

        _catalogItemRepository.Setup(s => s.UpdateAsync(
            It.IsAny<int>(),
            It.IsAny<string>(),
            It.IsAny<string>())).Returns((Func<ItemUpdateResponse>)null!);

        // act
        var result = await _catalogService.UpdateAsync(testId, testProperty, testValue);

        // assert
        result.Should().BeNull();
    }
}