namespace StockNexusAPI.Application.DTOs.Products
{
    public class ProductDto
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal UnitPrice { get; set; }
    }
}
