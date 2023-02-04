using System.Threading;
using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Response;
using Moq;

namespace Catalog.UnitTests.Services;

public class CatalogServiceTest
{
    private readonly ICatalogService _catalogService;

    private readonly Mock<ICatalogItemRepository> _catalogItemRepository;
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
    private readonly Mock<ILogger<CatalogService>> _logger;

    public CatalogServiceTest()
    {
        _catalogItemRepository = new Mock<ICatalogItemRepository>();
        _mapper = new Mock<IMapper>();
        _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
        _logger = new Mock<ILogger<CatalogService>>();

        var dbContextTransaction = new Mock<IDbContextTransaction>();
        _dbContextWrapper.Setup(s => s.BeginTransactionAsync(CancellationToken.None)).ReturnsAsync(dbContextTransaction.Object);

        _catalogService = new CatalogService(_dbContextWrapper.Object, _logger.Object, _catalogItemRepository.Object, _mapper.Object);
    }

    [Fact]
    public async Task GetCatalogItemsAsync_Success()
    {
        // arrange
        var testPageIndex = 0;
        var testPageSize = 4;
        var testTotalCount = 12;

        var pagingPaginatedItemsSuccess = new PaginatedItems<CatalogItem>()
        {
            Data = new List<CatalogItem>()
            {
                new CatalogItem()
                {
                    Name = "TestName",
                },
            },
            TotalCount = testTotalCount,
        };

        var catalogItemSuccess = new CatalogItem()
        {
            Name = "TestName"
        };

        var catalogItemDtoSuccess = new CatalogItemDto()
        {
            Name = "TestName"
        };

        _catalogItemRepository.Setup(s => s.GetByPageAsync(
            It.Is<int>(i => i == testPageIndex),
            It.Is<int>(i => i == testPageSize),
            It.IsAny<int?>(),
            It.IsAny<int?>())).ReturnsAsync(pagingPaginatedItemsSuccess);

        _mapper.Setup(s => s.Map<CatalogItemDto>(
            It.Is<CatalogItem>(i => i.Equals(catalogItemSuccess)))).Returns(catalogItemDtoSuccess);

        // act
        var result = await _catalogService.GetCatalogItemsAsync(testPageSize, testPageIndex, null);

        // assert
        result.Should().NotBeNull();
        result?.Data.Should().NotBeNull();
        result?.Count.Should().Be(testTotalCount);
        result?.PageIndex.Should().Be(testPageIndex);
        result?.PageSize.Should().Be(testPageSize);
    }

    [Fact]
    public async Task GetCatalogItemsAsync_Failed()
    {
        // arrange
        var testPageIndex = 1000;
        var testPageSize = 10000;

        _catalogItemRepository.Setup(s => s.GetByPageAsync(
            It.Is<int>(i => i == testPageIndex),
            It.Is<int>(i => i == testPageSize),
            It.IsAny<int?>(),
            It.IsAny<int?>())).Returns((Func<PaginatedItemsResponse<CatalogItemDto>>)null!);

        // act
        var result = await _catalogService.GetCatalogItemsAsync(testPageSize, testPageIndex, null);

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByIdAsync_Success()
    {
        // arrange
        var testId = 1;
        var catalogItemSuccess = new CatalogItem()
        {
            Name = "TestName"
        };
        var catalogItemDtoSuccess = new CatalogItemDto()
        {
            Name = "test"
        };

        _catalogItemRepository.Setup(s => s.GetByIdAsync(It.Is<int>(i => i == testId))).ReturnsAsync(catalogItemSuccess);
        _mapper.Setup(s => s.Map<CatalogItemDto>(It.Is<CatalogItem>(i => i.Equals(catalogItemSuccess)))).Returns(catalogItemDtoSuccess);

        // act
        var result = await _catalogService.GetByIdAsync(testId);

        // assert
        result.Should().NotBeNull();
        result?.Name.Should().NotBeNull();
    }

    [Fact]
    public async Task GetByIdAsync_Failed()
    {
        // arrange
        var testId = 100000;
        _catalogItemRepository.Setup(s => s.GetByIdAsync(It.Is<int>(i => i.Equals(testId)))).Returns((Func<GetItemByIdResponse>)null!);

        // act
        var result = await _catalogService.GetByIdAsync(testId);

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByBrandAsync_Success()
    {
        // arrange
        var testBrand = "TestName";
        var testPageIndex = 1;
        var testPageSize = 6;
        var testTotalCount = 6;

        var pagingPaginatedItemsSuccess = new PaginatedItems<CatalogItem>()
        {
            Data = new List<CatalogItem>()
            {
                new CatalogItem()
                {
                    Name = "TestName",
                },
            },
            TotalCount = testTotalCount,
        };
        var catalogItemSuccess = new CatalogItem()
        {
            Name = "TestName",
        };
        var catalogItemDtoSuccess = new CatalogItemDto()
        {
            Name = "TestName",
        };

        _catalogItemRepository.Setup(s => s.GetByBrandAsync(
            It.Is<string>(i => i.Equals(testBrand)),
            It.Is<int>(i => i.Equals(testPageIndex)),
            It.Is<int>(i => i.Equals(testPageSize)))).ReturnsAsync(pagingPaginatedItemsSuccess);

        _mapper.Setup(s => s.Map<CatalogItemDto>(
            It.Is<CatalogItem>(i => i.Equals(catalogItemSuccess)))).Returns(catalogItemDtoSuccess);

        // act
        var result = await _catalogService.GetByBrandAsync(testBrand, testPageIndex, testPageSize);

        // assert
        result.Should().NotBeNull();
        result?.Data.Should().NotBeNull();
        result?.Count.Should().Be(testTotalCount);
        result?.PageIndex.Should().Be(testPageIndex);
        result?.PageSize.Should().Be(testPageSize);
    }

    [Fact]
    public async Task GetByBrandAsync_Failed()
    {
        // arrange
        var testBrand = "test";
        var testPageIndex = 1000;
        var testPageSize = 10000;

        _catalogItemRepository.Setup(s => s.GetByBrandAsync(
            It.Is<string>(i => i.Equals(testBrand)),
            It.Is<int>(i => i == testPageIndex),
            It.Is<int>(i => i == testPageSize))).Returns((Func<PaginatedItemsResponse<CatalogItemDto>>)null!);

        // act
        var result = await _catalogService.GetByBrandAsync(testBrand, testPageSize, testPageIndex);

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByTypeAsync_Success()
    {
        // arrange
        var testType = "TestName";
        var testPageIndex = 1;
        var testPageSize = 6;
        var testTotalCount = 6;

        var pagingPaginatedItemsSuccess = new PaginatedItems<CatalogItem>()
        {
            Data = new List<CatalogItem>()
            {
                new CatalogItem()
                {
                    Name = "TestName",
                },
            },
            TotalCount = testTotalCount,
        };
        var catalogItemSuccess = new CatalogItem()
        {
            Name = "TestName",
        };
        var catalogItemDtoSuccess = new CatalogItemDto()
        {
            Name = "TestName",
        };

        _catalogItemRepository.Setup(s => s.GetByTypeAsync(
            It.Is<string>(i => i.Equals(testType)),
            It.Is<int>(i => i.Equals(testPageIndex)),
            It.Is<int>(i => i.Equals(testPageSize)))).ReturnsAsync(pagingPaginatedItemsSuccess);

        _mapper.Setup(s => s.Map<CatalogItemDto>(
            It.Is<CatalogItem>(i => i.Equals(catalogItemSuccess)))).Returns(catalogItemDtoSuccess);

        // act
        var result = await _catalogService.GetByTypeAsync(testType, testPageIndex, testPageSize);

        // assert
        result.Should().NotBeNull();
        result?.Data.Should().NotBeNull();
        result?.Count.Should().Be(testTotalCount);
        result?.PageIndex.Should().Be(testPageIndex);
        result?.PageSize.Should().Be(testPageSize);
    }

    [Fact]
    public async Task GetByTypeAsync_Failed()
    {
        // arrange
        var testType = "test";
        var testPageIndex = 1000;
        var testPageSize = 10000;

        _catalogItemRepository.Setup(s => s.GetByTypeAsync(
            It.Is<string>(i => i.Equals(testType)),
            It.Is<int>(i => i == testPageIndex),
            It.Is<int>(i => i == testPageSize))).Returns((Func<PaginatedItemsResponse<CatalogItemDto>>)null!);

        // act
        var result = await _catalogService.GetByTypeAsync(testType, testPageSize, testPageIndex);

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetBrandsAsync_Success()
    {
        var testPageIndex = 1;
        var testPageSize = 6;
        var testTotalCount = 6;

        var pagingPaginatedBrandsSuccess = new PaginatedItems<CatalogBrand>()
        {
            Data = new List<CatalogBrand>()
                {
                    new CatalogBrand()
                    {
                        Brand = "Test"
                    }
                },
            TotalCount = testTotalCount,
        };
        var catalogBrandSuccess = new CatalogBrand()
        {
            Brand = "Test"
        };
        var catalogBrandDtoSuccess = new CatalogBrandDto()
        {
            Brand = "Test"
        };

        _catalogItemRepository.Setup(s => s.GetBrandsAsync (
            It.Is<int>(i => i.Equals(testPageIndex)),
            It.Is<int>(i => i.Equals(testPageSize)))).ReturnsAsync(pagingPaginatedBrandsSuccess);

        _mapper.Setup(s => s.Map<CatalogBrandDto>(
            It.Is<CatalogBrand>(i => i.Equals(catalogBrandSuccess)))).Returns(catalogBrandDtoSuccess);

    // act
        var result = await _catalogService.GetBrandsAsync(testPageIndex, testPageSize);

    // assert
        result.Should().NotBeNull();
        result?.Data.Should().NotBeNull();
        result?.Count.Should().Be(testTotalCount);
        result?.PageIndex.Should().Be(testPageIndex);
        result?.PageSize.Should().Be(testPageSize);
    }

    [Fact]
    public async Task GetBrandsAsync_Failed()
    {
        // arrange
        var testPageIndex = 1000;
        var testPageSize = 10000;

        _catalogItemRepository.Setup(s => s.GetBrandsAsync(
            It.Is<int>(i => i == testPageIndex),
            It.Is<int>(i => i == testPageSize))).Returns((Func<PaginatedItemsResponse<CatalogBrandDto>>)null!);

        // act
        var result = await _catalogService.GetBrandsAsync(testPageSize, testPageIndex);

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetTypesAsync_Success()
    {
        var testPageIndex = 1;
        var testPageSize = 6;
        var testTotalCount = 6;

        var pagingPaginatedTypesSuccess = new PaginatedItems<CatalogType>()
        {
            Data = new List<CatalogType>()
                {
                    new CatalogType()
                    {
                        Type = "Test"
                    }
                },
            TotalCount = testTotalCount,
        };
        var catalogTypeSuccess = new CatalogType()
        {
            Type = "Test"
        };
        var catalogTypeDtoSuccess = new CatalogTypeDto()
        {
            Type = "Test"
        };

        _catalogItemRepository.Setup(s => s.GetTypesAsync(
            It.Is<int>(i => i.Equals(testPageIndex)),
            It.Is<int>(i => i.Equals(testPageSize)))).ReturnsAsync(pagingPaginatedTypesSuccess);

        _mapper.Setup(s => s.Map<CatalogTypeDto>(
            It.Is<CatalogType>(i => i.Equals(catalogTypeSuccess)))).Returns(catalogTypeDtoSuccess);

        // act
        var result = await _catalogService.GetTypesAsync(testPageIndex, testPageSize);

        // assert
        result.Should().NotBeNull();
        result?.Data.Should().NotBeNull();
        result?.Count.Should().Be(testTotalCount);
        result?.PageIndex.Should().Be(testPageIndex);
        result?.PageSize.Should().Be(testPageSize);
    }

    [Fact]
    public async Task GetTypesAsync_Failed()
    {
        // arrange
        var testPageIndex = 1000;
        var testPageSize = 10000;

        _catalogItemRepository.Setup(s => s.GetTypesAsync(
            It.Is<int>(i => i == testPageIndex),
            It.Is<int>(i => i == testPageSize))).Returns((Func<PaginatedItemsResponse<CatalogTypeDto>>)null!);

        // act
        var result = await _catalogService.GetTypesAsync(testPageSize, testPageIndex);

        // assert
        result.Should().BeNull();
    }
}