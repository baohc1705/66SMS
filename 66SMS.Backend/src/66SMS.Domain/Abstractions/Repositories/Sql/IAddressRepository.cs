using _66SMS.Domain.Entities;

namespace _66SMS.Domain.Abstractions.Repositories.Sql
{
    public interface IAddressRepository
    {
        Task<List<Address>> GetAddressesByUserIdAsync(Guid userId, CancellationToken ct = default);
        Task<Address?> GetByUserIdAndAddressIdAsync(Guid userId,Guid addressId, CancellationToken ct = default);
        Task<Guid> AddOrUpdateAddressAsync(Address address, CancellationToken ct = default);
        Task<bool> DeleteAddressAsync(Guid userId, Guid addressId, CancellationToken ct = default);
    }
}
