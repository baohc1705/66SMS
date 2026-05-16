using _66SMS.Domain.Entities;
using _66SMS.Persistence.Configurations.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace _66SMS.Persistence.Repositories.Sql.Base
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Address> Addresses { get; set; } = null!;
        public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            ConfigIdentityEntity(builder);

            SeedDataIdentity(builder);
        }

        private void ConfigIdentityEntity(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<ApplicationRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins");
            builder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens");
        }

        private void SeedDataIdentity(ModelBuilder builder)
        {
            var adminRoleId = Guid.Parse("c4a3298c-6198-4d12-bd1a-56d1d1ce0aa7");
            var customerRoleId = Guid.Parse("38b657f4-ac20-4a5c-b2a3-16dfad61c381");
            var vendorRoleId = Guid.Parse("582880c3-f554-490f-a24e-526db35cffa5");
            builder.Entity<ApplicationRole>().HasData(
               new ApplicationRole
               {
                   Id = adminRoleId,
                   Name = "Admin",
                   NormalizedName = "ADMIN",
                   Description = "Administrator with full permissions"
               },
               new ApplicationRole
               {
                   Id = customerRoleId,
                   Name = "Customer",
                   NormalizedName = "CUSTOMER",
                   Description = "Customer with shopping permissions"
               },
               new ApplicationRole
               {
                   Id = vendorRoleId,
                   Name = "Vendor",
                   NormalizedName = "VENDOR",
                   Description = "Vendor who can manage products"
               }
           );
        }
    }
}
