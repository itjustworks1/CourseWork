using Magaz_Stroitelya.VMTools;

namespace MVVM.Model.DTO.Response
{
    public class ProductRequest
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public decimal Value { get; set; }
        public int Quantity { get; set; }
        public int ProductTypeId { get; set; }
    }
}
