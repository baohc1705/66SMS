namespace _66SMS.Application.DTOs.Identity.Queries
{
    public class EmailConfirmationTokenResponseDTO
    {
        public Guid UserId { get; set; }
        public string Token { get; set; } = null!;
    }
}
