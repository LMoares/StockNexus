using MediatR;
using StockNexusAPI.Application.DTOs.Products;

namespace StockNexusAPI.Application.Features.Product.Queries.GetProducts
{
    public class GetProductsQuery : IRequest<IReadOnlyList<ProductDto>>
    {
    }
}
