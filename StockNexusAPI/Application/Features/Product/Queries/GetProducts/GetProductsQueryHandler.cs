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
            var query = _context.Products.AsNoTracking();

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                query = query.Where(p => p.Description.Contains(request.SearchTerm));
            }

            var items = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    UnitPrice = p.UnitPrice
                })
                .ToListAsync(token);

            return items;
        }
    }
}
