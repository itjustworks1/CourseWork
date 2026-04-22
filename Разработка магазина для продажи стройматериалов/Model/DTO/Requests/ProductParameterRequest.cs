namespace MVVM.Model.DTO.Response
{
    public class ProductParameterRequest
    {
        public int Id { get; set; }
        public string Meaning { get; set; } = null!;
        public int ParameterId { get; set; }
        public int ProductId { get; set; }
    }
}
