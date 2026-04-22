namespace API.DTO.Auth
{
    public class LoginResponse
    {        
        public string Token { get; set; } = null!;
        public int UserId { get; set; }
        //public int ExpiresIn { get; set; }
    }
}
