namespace garagesales.Models.dto
{
    public class GarageSaleItemDto
    {
        public int GarageSaleId { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public decimal Price { get; set; }
    }
}
