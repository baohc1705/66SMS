namespace _66SMS.Application.DTOs.Identity.Queries
{
    public class ProfileDTO
    {
        public Guid UserId { get; set; }
        public string? FullName { get; set; } = null!;
        public string? PhoneNumber { get; set; } = null!;
        public string? Email { get; set; } = null!;
        public string? UserName { get; set; } = null!;
        public DateTime? LastLoginAt { get; set; }
        public string? ProfilePhotoUrl { get; set; }
    }
}
