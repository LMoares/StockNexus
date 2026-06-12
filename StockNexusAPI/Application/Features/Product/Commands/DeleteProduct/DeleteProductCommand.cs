using MediatR;

namespace StockNexusAPI.Application.Features.Product.Commands.DeleteProduct
{
    public class DeleteProductCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
