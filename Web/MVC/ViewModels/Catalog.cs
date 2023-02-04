namespace MVC.ViewModels;

public record Catalog<T>
{
    public int PageIndex { get; init; }
    public int PageSize { get; init; }
    public int Count { get; init; }
    public List<T> Data { get; init; }
}
