using StockNexusAPI.Domain.Enums;

namespace StockNexusAPI.Domain.Entities
{
    public class ProductRequest
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public int Quantity { get; set; }

        public RequestStatus Status { get; set; }
        public string? ManagerRemarks { get; set; }

        public int EmployeeId { get; set; }
        public User Employee { get; set; } = null!;

        public int ManagerId { get; set; }
        public User Manager { get; set; } = null!;

        public DateTime CreatedAt { get; set; } 
        public DateTime? ReviewedAt { get; set; }

    }
}
