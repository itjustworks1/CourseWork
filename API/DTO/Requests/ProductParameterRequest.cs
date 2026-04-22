namespace API.DTO.Requests
{
    public class ProductParameterRequest
    {
        public string Meaning { get; set; } = null!;
        public int ParameterId { get; set; }
        public int ProductId { get; set; }
    }
}
