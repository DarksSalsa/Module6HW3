namespace Catalog.Host.Models.Requests;

public class PaginatedItemsByBrandRequest
{
    public int PageIndex { get; set; }

    public int PageSize { get; set; }

    public string Brand { get; set; } = null!;
}