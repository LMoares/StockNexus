using MediatR;
using StockNexusAPI.Infrastructure.Persistence;

namespace StockNexusAPI.Application.Features.Product.Commands.DeleteProduct
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Unit>
    {
        private readonly ApplicationDbContext _context;

        public DeleteProductCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteProductCommand command, CancellationToken token)
        {
            var product = await _context.Products.FindAsync(new object[] { command.Id }, token);
            if (product == null)
            {
                throw new KeyNotFoundException($"Product with Id {command.Id} not found.");
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync(token);
            return Unit.Value;
        }
    }
}
