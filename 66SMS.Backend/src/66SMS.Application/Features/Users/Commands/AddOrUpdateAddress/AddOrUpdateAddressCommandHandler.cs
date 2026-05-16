using _66SMS.Application.DTOs.Address;
using _66SMS.Domain.Abstractions.Repositories.Sql;
using _66SMS.Domain.Entities;
using MediatR;

using _66SMS.Contracts.Shared;

namespace _66SMS.Application.Features.Users.Commands.AddOrUpdateAddress
{
    public sealed record AddOrUpdateAddressCommand(
        Guid? Id, 
        Guid UserId, 
        string City, 
        string Ward, 
        string Street, 
        bool IsDefaultShipping, 
        bool IsDefaultBilling) : IRequest<Result<Guid>>;

    public sealed class AddOrUpdateAddressCommandHandler : IRequestHandler<AddOrUpdateAddressCommand, Result<Guid>>
    {
        private readonly IAddressRepository addressRepo;
        private readonly IUserSqlRepository userRepo;

        public AddOrUpdateAddressCommandHandler(
            IAddressRepository addressRepo,
            IUserSqlRepository userRepo)
        {
            this.addressRepo = addressRepo;
            this.userRepo = userRepo;
        }

        public async Task<Result<Guid>> Handle(AddOrUpdateAddressCommand request, CancellationToken cancellationToken)
        {
            var userExists = await userRepo.IsUserExistsAsync(request.UserId, cancellationToken);
            if (!userExists)
                return Result<Guid>.NotFound("User not found.");

            var address = new Address
            {
                Id = request.Id ?? Guid.NewGuid(),
                UserId = request.UserId,
                City = request.City,
                Ward = request.Ward,
                Street = request.Street,
                IsDefaultShipping = request.IsDefaultShipping,
                IsDefaultBilling = request.IsDefaultBilling
            };

            var addressId = await addressRepo.AddOrUpdateAddressAsync(address, cancellationToken);
            if (addressId == Guid.Empty)
                return Result<Guid>.BadRequest("Failed to save address.");
                
            return Result<Guid>.Success(addressId, "Address saved successfully.");
        }
    }
}
