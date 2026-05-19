using MediatR;
using Microsoft.EntityFrameworkCore;
using StockNexusAPI.Application.DTOs.Products;
using StockNexusAPI.Infrastructure.Persistence;

namespace StockNexusAPI.Application.Features.Product.Queries.GetProducts
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IReadOnlyList<ProductDto>>
    {
        private readonly ApplicationDbContext _context;

        public GetProductsQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<ProductDto>> Handle(GetProductsQuery request, CancellationToken token)
        {
            return await _context.Products
                .AsNoTracking()
                .Select(p => new ProductDto
                {
                    Name = p.Name,
                    Description = p.Description,
                    UnitPrice = p.UnitPrice
                }).ToListAsync(token);
        }
    }
}
