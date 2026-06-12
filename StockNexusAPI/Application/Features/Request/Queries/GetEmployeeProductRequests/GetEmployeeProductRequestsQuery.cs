using MediatR;
using StockNexusAPI.Application.DTOs.Requests;
using StockNexusAPI.Domain.Enums;

namespace StockNexusAPI.Application.Features.ProductRequest.Queries.GetEmployeeProductRequests
{
    public class GetEmployeeProductRequestsQuery : IRequest<IReadOnlyList<ProductRequestDto>>
    {
        public RequestStatus? StatusFilter { get; set; }
    }
}
