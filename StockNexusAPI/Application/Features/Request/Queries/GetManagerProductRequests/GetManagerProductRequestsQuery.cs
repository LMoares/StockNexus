using MediatR;
using StockNexusAPI.Application.DTOs.Requests;
using StockNexusAPI.Domain.Enums;

namespace StockNexusAPI.Application.Features.ProductRequest.Queries.GetManagerProductRequests
{
    public class GetManagerProductRequestsQuery : IRequest<IReadOnlyList<ProductRequestDto>>
    {
        public RequestStatus? StatusFilter { get; set; }
    }
}
