using _66SMS.Domain.Entities;
using _66SMS.Persistence.Configurations.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _66SMS.Persistence.Configurations.Sql
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable(nameof(Address));

            builder.HasOne<ApplicationUser>().WithMany(x => x.Addresses).HasForeignKey(x => x.UserId).IsRequired(false);
        }
    }
}
