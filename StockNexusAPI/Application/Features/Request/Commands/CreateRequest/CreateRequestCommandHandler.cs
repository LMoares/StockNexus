using MediatR;
using Microsoft.EntityFrameworkCore;
using StockNexusAPI.Application.Common.Interfaces;
using StockNexusAPI.Domain.Entities;
using StockNexusAPI.Domain.Enums;
using StockNexusAPI.Infrastructure.Persistence;

namespace StockNexusAPI.Application.Features.Request.Commands.CreateRequest
{
    public class CreateRequestCommandHandler : IRequestHandler<CreateRequestCommand, int>
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        public CreateRequestCommandHandler(ApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }
        public async Task<int> Handle(CreateRequestCommand request, CancellationToken token)
        {
            if (!_currentUserService.IsAuthenticated || !_currentUserService.UserId.HasValue) {
                throw new InvalidOperationException("Only authenticated users can submit requests");
            }

            var employee = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == _currentUserService.UserId.Value, token);

            if (employee == null) throw new KeyNotFoundException("Employee not found");
            if (employee.ManagerId == null) throw new InvalidOperationException("Requests cannot be submitted by employees without a manager");

            var productRequest = new ProductRequest
            {
                ProductId = request.ProductId,
                Quantity = request.Quantity,
                EmployeeId = _currentUserService.UserId.Value,
                ManagerId = employee.ManagerId.Value,
                Status = RequestStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            _context.ProductRequests.Add(productRequest);
            await _context.SaveChangesAsync(token);

            return productRequest.Id;
        }
    }
}
