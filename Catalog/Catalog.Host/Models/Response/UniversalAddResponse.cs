namespace Catalog.Host.Models.Response;

public class UniversalAddResponse<T>
{
    public T Id { get; set; } = default(T) !;
}