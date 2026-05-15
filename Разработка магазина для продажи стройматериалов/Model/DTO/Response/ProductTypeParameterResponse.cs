namespace MVVM.Model.DTO.Response
{
    public class ProductTypeParameterResponse
    {
        public int ProductTypeId { get; set; }
        public ProductTypeResponse ProductType { get; set; }
        public int ParameterId { get; set; }
        public ParameterResponse Parameter { get; set; }
    }
}
