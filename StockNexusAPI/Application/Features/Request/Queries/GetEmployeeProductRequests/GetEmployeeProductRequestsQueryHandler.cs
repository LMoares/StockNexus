using MediatR;
using Microsoft.EntityFrameworkCore;
using StockNexusAPI.Application.Common.Interfaces;
using StockNexusAPI.Application.DTOs.Requests;
using StockNexusAPI.Infrastructure.Persistence;

namespace StockNexusAPI.Application.Features.ProductRequest.Queries.GetEmployeeProductRequests
{
    public class GetEmployeeProductRequestsQueryHandler : IRequestHandler<GetEmployeeProductRequestsQuery, IReadOnlyList<ProductRequestDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public GetEmployeeProductRequestsQueryHandler(ApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<IReadOnlyList<ProductRequestDto>> Handle(GetEmployeeProductRequestsQuery request, CancellationToken token)
        {
            if (!_currentUserService.IsAuthenticated || !_currentUserService.UserId.HasValue) {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            var employeeId = _currentUserService.UserId.Value;

            var query = _context.ProductRequests
                .AsNoTracking()
                .Where(x => x.EmployeeId == employeeId);

            if (request.StatusFilter.HasValue)
            {
                query = query.Where(x => x.Status == request.StatusFilter.Value);
            }

            return await query
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new ProductRequestDto
                {
                    Id = x.Id,
                    ProductName = x.Product.Name,
                    Quantity = x.Quantity,
                    Status = x.Status.ToString(),
                    EmployeeName = $"{x.Employee.FirstName} {x.Employee.LastName}",
                    ManagerName = $"{x.Manager.FirstName} {x.Manager.LastName}",
                    ManagerRemarks = x.ManagerRemarks,
                    CreatedAt = x.CreatedAt,
                    ReviewedAt = x.ReviewedAt
                })
                .ToListAsync(token);
        }
    }
}
