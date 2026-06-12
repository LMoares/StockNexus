using MediatR;
using Microsoft.EntityFrameworkCore;
using StockNexusAPI.Domain.Enums;
using StockNexusAPI.Infrastructure.Persistence;

namespace StockNexusAPI.Application.Features.Request.Commands.ReviewRequest
{
    public class ReviewRequestCommandHandler : IRequestHandler<ReviewRequestCommand, Unit>
    {
        private readonly ApplicationDbContext _context;

        public ReviewRequestCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(ReviewRequestCommand request, CancellationToken token)
        {
            var productRequest = await _context.ProductRequests
                .FirstOrDefaultAsync(x => x.Id == request.RequestId, token);

            if (productRequest == null) throw new KeyNotFoundException("Request not found.");

            if (productRequest.ManagerId != request.ManagerId) throw new UnauthorizedAccessException("You are not authorized to review this request.");

            if (productRequest.Status != RequestStatus.Pending) throw new InvalidOperationException("Only pending requests can be reviewed.");

            productRequest.Status = request.Approve ? RequestStatus.Approved : RequestStatus.Denied;
            productRequest.ManagerRemarks = request.Remarks;
            productRequest.ReviewedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(token);
            return Unit.Value;
        }
    }
}
