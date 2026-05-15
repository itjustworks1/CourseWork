namespace API.DTO.Requests
{
    public class OrderRequest
    {
        public DateTime Date { get; set; }
        public bool Status { get; set; }
        public int UserId { get; set; }
    }
}
