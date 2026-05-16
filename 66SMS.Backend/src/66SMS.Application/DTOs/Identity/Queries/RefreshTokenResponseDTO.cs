namespace _66SMS.Application.DTOs.Identity.Queries
{
    public class RefreshTokenResponseDTO
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
