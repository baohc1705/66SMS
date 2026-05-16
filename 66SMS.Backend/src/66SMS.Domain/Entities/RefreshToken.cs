using _66SMS.Domain.Abstractions.Entities;

namespace _66SMS.Domain.Entities
{
    public class RefreshToken : EntityAuditTable<Guid>
    {
        public Guid UserId { get; set; }
        public string Token { get; set; } = null!;
        public string UserAgent { get; set; } = null!;   // Chrome, Firefox, Safari, etc.
        public string CreatedByIp { get; set; } = null!;
        public DateTime? RevokedAt { get; set; }
        public string? RevokedByIp { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsActive => RevokedAt == null && !IsExpired;
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    }
}
