namespace Catalog.Host.Models.Requests
{
    public class UniversalUpdateRequest
    {
        public int Id { get; set; }

        public string Property { get; set; } = null!;

        public string Value { get; set; } = null!;
    }
}
