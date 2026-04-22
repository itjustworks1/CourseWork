namespace API.DTO.Raquests
{
    public class OrderStructureRequest
    {
        public int Quantity { get; set; }
        public decimal Value { get; set; }
        public int ProductId { get; set; }
        public int OrderId { get; set; }
    }
}
