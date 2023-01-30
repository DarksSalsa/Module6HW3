using Catalog.Host.Models.Dtos;

namespace Catalog.Host.Models.Response
{
    public class GetItemByIdResponse
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public decimal Price { get; set; }

        public string PictureUrl { get; set; } = null!;

        public CatalogTypeDto CatalogType { get; set; } = new CatalogTypeDto() { };

        public CatalogBrandDto CatalogBrand { get; set; } = new CatalogBrandDto() { };

        public int AvailableStock { get; set; }
    }
}
