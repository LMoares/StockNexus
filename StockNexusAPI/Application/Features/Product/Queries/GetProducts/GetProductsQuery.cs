using MediatR;
using StockNexusAPI.Application.DTOs.Products;

namespace StockNexusAPI.Application.Features.Product.Queries.GetProducts
{
    public class GetProductsQuery : IRequest<IReadOnlyList<ProductDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10; // 10 items per page by default
        public string? SearchTerm { get; set; } // Filtering by description
    
    }
}
