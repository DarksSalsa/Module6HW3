namespace Catalog.Host.Models.Requests;

public class PaginatedItemsByTypeRequest
{
    public int PageIndex { get; set; }

    public int PageSize { get; set; }

    public string Type { get; set; } = null!;
}