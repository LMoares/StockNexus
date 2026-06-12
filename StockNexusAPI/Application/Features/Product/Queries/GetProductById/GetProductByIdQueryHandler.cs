using MediatR;
using Microsoft.EntityFrameworkCore;
using StockNexusAPI.Application.DTOs.Products;
using StockNexusAPI.Infrastructure.Persistence;

namespace StockNexusAPI.Application.Features.Product.Queries.GetProductById
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto>
    {
        private readonly ApplicationDbContext _context;
        public GetProductByIdQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken token)
        {
            var product = await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == request.Id, token);
            if (product == null)
            {
                throw new KeyNotFoundException($"Product with Id {request.Id} not found.");
            }
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                UnitPrice = product.UnitPrice
            };
        }
    }
}
