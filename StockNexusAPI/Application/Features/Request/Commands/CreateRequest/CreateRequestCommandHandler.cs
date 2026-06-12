using MediatR;
using Microsoft.EntityFrameworkCore;
using StockNexusAPI.Domain.Entities;
using StockNexusAPI.Domain.Enums;
using StockNexusAPI.Infrastructure.Persistence;

namespace StockNexusAPI.Application.Features.Request.Commands.CreateRequest
{
    public class CreateRequestCommandHandler : IRequestHandler<CreateRequestCommand, int>
    {
        private readonly ApplicationDbContext _context;
        public CreateRequestCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<int> Handle(CreateRequestCommand request, CancellationToken token)
        {
            var employee = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.EmployeeId, token);

            if (employee == null) throw new KeyNotFoundException("Employee not found");
            if (employee.ManagerId == null) throw new InvalidOperationException("Requests cannot be submitted by employees without a manager");

            var productRequest = new ProductRequest
            {
                ProductId = request.ProductId,
                Quantity = request.Quantity,
                EmployeeId = request.EmployeeId,
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
