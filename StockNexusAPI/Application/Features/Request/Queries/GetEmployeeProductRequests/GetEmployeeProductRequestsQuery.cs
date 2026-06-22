using MediatR;
using StockNexusAPI.Application.DTOs.Requests;
using StockNexusAPI.Domain.Enums;

namespace StockNexusAPI.Application.Features.ProductRequest.Queries.GetEmployeeProductRequests
{
    public class GetEmployeeProductRequestsQuery : IRequest<IReadOnlyList<ProductRequestDto>>
    {
        public RequestStatus? StatusFilter { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10; // 10 items per page by default
        
    }
}
