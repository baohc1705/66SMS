using Microsoft.AspNetCore.Identity;

namespace _66SMS.Persistence.Configurations.Identity
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public string? Description { get; set; }
    }
}
