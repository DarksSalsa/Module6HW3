namespace Catalog.Host.Models.Requests;

public class PaginatedRequest
{
    public int PageIndex { get; set; }

    public int PageSize { get; set; }
}