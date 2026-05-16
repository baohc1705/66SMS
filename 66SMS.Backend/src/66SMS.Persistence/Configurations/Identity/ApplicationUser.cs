using _66SMS.Domain.Abstractions.Entities.Base;
using _66SMS.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace _66SMS.Persistence.Configurations.Identity
{
    public class ApplicationUser : IdentityUser<Guid>, IDateTracking, ISoftDelete
    {
        
        public bool IsActive { get; set; } = true;
        public string? FullName { get; set; }
        public DateTime? DoB { get; set; }
        public string? ProfilePhotoUrl { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get ; set ; }
        public bool IsDeleted { get ; set ; }
        public DateTime? DeletedAt { get; set; }
        public ICollection<Address> Addresses { get; set; } = new List<Address>();
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
