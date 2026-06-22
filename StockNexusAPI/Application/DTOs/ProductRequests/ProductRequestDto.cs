namespace StockNexusAPI.Application.DTOs.Requests
{
    public class ProductRequestDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string Status { get; set; } = string.Empty;

        public string EmployeeName { get; set; } = string.Empty;
        public string ManagerName { get; set; } = string.Empty;

        public string? ManagerRemarks { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ReviewedAt { get; set; }
    }
}
