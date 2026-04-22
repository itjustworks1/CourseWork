namespace MVVM.Model.DTO.Response
{
    public class ProductParameterResponse
    {
        public int Id { get; set; }
        public string Meaning { get; set; } = null!;
        public int ParameterId { get; set; }
        public ParameterResponse Parameter { get; set; }
        public int ProductId { get; set; }
        public ProductResponse Product { get; set; }
    }
}
