using _66SMS.Application.DTOs.Address;
using _66SMS.Domain.Abstractions.Repositories.Sql;
using MediatR;

using _66SMS.Contracts.Shared;

namespace _66SMS.Application.Features.Users.Queries.GetAddressById
{
    public sealed record GetAddressByIdQuery(Guid UserId, Guid AddressId) : IRequest<Result<AddressDTO>>;

    public sealed class GetAddressByIdQueryHandler : IRequestHandler<GetAddressByIdQuery, Result<AddressDTO>>
    {
        private readonly IAddressRepository addressRepo;

        public GetAddressByIdQueryHandler(IAddressRepository addressRepo)
        {
            this.addressRepo = addressRepo;
        }

        public async Task<Result<AddressDTO>> Handle(GetAddressByIdQuery request, CancellationToken cancellationToken)
        {
            var address = await addressRepo.GetByUserIdAndAddressIdAsync(
                request.UserId, request.AddressId, cancellationToken);

            if (address == null)
                return Result<AddressDTO>.NotFound("Address not found.");

            return Result<AddressDTO>.Success(new AddressDTO
            {
                Id = address.Id,
                userId = address.UserId,
                City = address.City,
                Ward = address.Ward,
                Street = address.Street,
                IsDefaultShipping = address.IsDefaultShipping,
                IsDefaultBilling = address.IsDefaultBilling
            });
        }
    }
}
