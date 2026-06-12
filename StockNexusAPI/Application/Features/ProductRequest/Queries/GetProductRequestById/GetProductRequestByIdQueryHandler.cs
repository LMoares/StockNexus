using MediatR;
using Microsoft.EntityFrameworkCore;
using StockNexusAPI.Application.Common.Interfaces;
using StockNexusAPI.Application.DTOs.Requests;
using StockNexusAPI.Infrastructure.Persistence;

namespace StockNexusAPI.Application.Features.Request.Queries.GetRequestById
{
    public class GetProductRequestByIdQueryHandler : IRequestHandler<GetProductRequestByIdQuery, ProductRequestDto?>
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public GetProductRequestByIdQueryHandler(ApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<ProductRequestDto?> Handle(GetProductRequestByIdQuery request, CancellationToken token)
        {
            var productRequest = await _context.ProductRequests
                .AsNoTracking()
                .Include(x => x.Product)
                .Include(x => x.Employee)
                .Include(x => x.Manager)
                .FirstOrDefaultAsync(x => x.Id == request.Id, token);

            if (productRequest == null) return null;

            var currentUserId = _currentUserService.UserId;

            bool isOwner = productRequest.EmployeeId == currentUserId;
            bool isAssignedManager = productRequest.ManagerId == currentUserId;

            if (!isOwner && !isAssignedManager)
            {
                // User is neither the owner nor the assigned manager, return null or throw an exception
                return null;
            }

            return new ProductRequestDto
            {
                Id = productRequest.Id,
                ProductName = productRequest.Product.Name,
                Quantity = productRequest.Quantity,
                Status = productRequest.Status.ToString(),
                EmployeeName = $"{productRequest.Employee.FirstName} {productRequest.Employee.LastName}",
                ManagerName = $"{productRequest.Manager.FirstName} {productRequest.Manager.LastName}",
                ManagerRemarks = productRequest.ManagerRemarks,
                CreatedAt = productRequest.CreatedAt,
                ReviewedAt = productRequest.ReviewedAt
            };
        }
    }
}
