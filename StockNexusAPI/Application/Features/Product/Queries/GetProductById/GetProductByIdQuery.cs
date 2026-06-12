using MediatR;
using StockNexusAPI.Application.DTOs.Products;

namespace StockNexusAPI.Application.Features.Product.Queries.GetProductById
{
    public class GetProductByIdQuery : IRequest<ProductDto>
    {
        public int Id { get; set; }
    }
}
