using _66SMS.Domain.Abstractions.Repositories.Sql;
using _66SMS.Domain.Entities;
using _66SMS.Persistence.Repositories.Sql.Base;
using Microsoft.EntityFrameworkCore;

namespace _66SMS.Persistence.Repositories.Sql
{
    public class AddressRepository : IAddressRepository
    {
        private readonly ApplicationDbContext context;

        public AddressRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Guid> AddOrUpdateAddressAsync(Address address, CancellationToken ct = default)
        {
            var existing = await context.Addresses.FindAsync(address.Id);
            if (existing == null)
            {
                await context.Addresses.AddAsync(address);
                await context.SaveChangesAsync();
                return address.Id; // New address Id
            }
            else
            {
               
                existing.City = address.City;
                existing.Ward = address.Ward;
                existing.Street = address.Street;
                existing.IsDefaultBilling = address.IsDefaultBilling;
                existing.IsDefaultShipping = address.IsDefaultShipping;
                await context.SaveChangesAsync();
                return existing.Id; // Existing address Id
            }
        }

        public async Task<bool> DeleteAddressAsync(Guid userId, Guid addressId, CancellationToken ct = default)
        {
            var address = await context.Addresses.FirstOrDefaultAsync(a => a.Id == addressId && a.UserId == userId);
            if (address == null)
                return false;

            context.Addresses.Remove(address);
            await context.SaveChangesAsync();
            return true;

        }

        public async Task<List<Address>> GetAddressesByUserIdAsync(Guid userId, CancellationToken ct = default)
        {
            return await context.Addresses.Where(a => a.UserId == userId).ToListAsync();

        }

        public async Task<Address?> GetByUserIdAndAddressIdAsync(Guid userId, Guid addressId, CancellationToken ct = default)
        {
            return await context.Addresses.AsNoTracking().Where(x => x.UserId.Equals(userId) && x.Id.Equals(addressId)).FirstOrDefaultAsync(ct);
        }
    }
}
