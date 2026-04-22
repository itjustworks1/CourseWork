namespace API.DTO.Raquests
{
    public class ProductRequest
    {
        public string Title { get; set; } = null!;
        public decimal Value { get; set; }
        public int Quantity { get; set; }
        public int ProductTypeId { get; set; }
    }
}
