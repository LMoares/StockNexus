using MediatR;
using System.Text.Json.Serialization;

namespace StockNexusAPI.Application.Features.Request.Commands.ReviewRequest
{
    public class ReviewRequestCommand : IRequest<Unit>
    {
        public int RequestId { get; set; }
        public bool Approve { get; set; }
        public string? Remarks { get; set; }
    }
}
