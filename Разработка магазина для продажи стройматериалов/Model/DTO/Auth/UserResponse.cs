namespace Magaz_Stroitelya.DTO.Auth
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string Login { get; set; } = null!;
        public bool IsAdmin { get; set; } = false!;
    }
}
