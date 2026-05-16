using _66SMS.Application.DTOs.Address;
using _66SMS.Domain.Abstractions.Repositories.Sql;
using MediatR;

using _66SMS.Contracts.Shared;

namespace _66SMS.Application.Features.Users.Queries.GetAddresses
{
    public sealed record GetAddressesQuery(Guid UserId) : IRequest<Result<List<AddressDTO>>>;

    public sealed class GetAddressesQueryHandler : IRequestHandler<GetAddressesQuery, Result<List<AddressDTO>>>
    {
        private readonly IAddressRepository addressRepo;

        public GetAddressesQueryHandler(IAddressRepository addressRepo)
        {
            this.addressRepo = addressRepo;
        }

        public async Task<Result<List<AddressDTO>>> Handle(GetAddressesQuery request, CancellationToken cancellationToken)
        {
            var addresses = await addressRepo.GetAddressesByUserIdAsync(request.UserId, cancellationToken);

            var dtos = addresses.Select(a => new AddressDTO
            {
                Id = a.Id,
                userId = a.UserId,
                City = a.City,
                Ward = a.Ward,
                Street = a.Street,
                IsDefaultShipping = a.IsDefaultShipping,
                IsDefaultBilling = a.IsDefaultBilling
            }).ToList();

            return Result<List<AddressDTO>>.Success(dtos);
        }
    }
}
