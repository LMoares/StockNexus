using MediatR;
using StockNexusAPI.Application.DTOs.Requests;

namespace StockNexusAPI.Application.Features.Request.Queries.GetRequestById
{
    public class GetProductRequestByIdQuery : IRequest<ProductRequestDto?>
    {
        public int Id { get; set; }
    }
}
