using MediatR;
using StockNexusAPI.Infrastructure.Persistence;

namespace StockNexusAPI.Application.Features.Product.Commands.RegisterProduct
{
    public class RegisterProductCommandHandler
    {
        private readonly ApplicationDbContext _context;

        public RegisterProductCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(RegisterProductCommand command, CancellationToken token)
        {
            var product = new Domain.Entities.Product
            {
                Name = command.Name,
                Description = command.Description,
                UnitPrice = command.UnitPrice
            };
            _context.Products.Add(product);
            await _context.SaveChangesAsync(token);

            return Unit.Value;
        }
    }
}
