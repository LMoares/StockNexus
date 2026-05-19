using MediatR;
namespace StockNexusAPI.Application.Features.Product.Commands.RegisterProduct
{
    public class RegisterProductCommand : IRequest<Unit>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
    }
}
