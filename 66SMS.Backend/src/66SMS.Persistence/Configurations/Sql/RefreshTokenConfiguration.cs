using _66SMS.Domain.Entities;
using _66SMS.Persistence.Configurations.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _66SMS.Persistence.Configurations.Sql
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(x => x.Id);

            builder.ToTable(nameof(RefreshToken));

            builder.HasOne<ApplicationUser>().WithMany(x => x.RefreshTokens).HasForeignKey(x => x.UserId).IsRequired(false);
        }
    }
}
