namespace MVC.ViewModels
{
    public class SupplementaryCollectionModel<T>
    {
        public long Count { get; set; }
        public IEnumerable<T> Data { get; set; } = new List<T>();
    }
}
