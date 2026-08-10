namespace garagesales.Models.dto
{
    public class GarageSaleDto
    {
        public string? UserId { get; set; }
        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Address { get; set; } = null!;

        public string City { get; set; } = null!;

        public string State { get; set; } = null!;

        public string ZipCode { get; set; } = null!;

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}
