using MediatR;
using System.Text.Json.Serialization;

namespace StockNexusAPI.Application.Features.Request.Commands.CreateRequest
{
    public class CreateRequestCommand : IRequest<int>
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        [JsonIgnore] // EmployeeId retrieved via JWT decoding, not via client input
        public int EmployeeId { get; set; }
    }
}
