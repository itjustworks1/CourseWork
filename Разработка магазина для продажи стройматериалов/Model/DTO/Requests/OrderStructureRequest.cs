namespace MVVM.Model.DTO.Response
{
    public class OrderStructureRequest
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal Value { get; set; }
        public int ProductId { get; set; }
        public int OrderId { get; set; }
    }
}
