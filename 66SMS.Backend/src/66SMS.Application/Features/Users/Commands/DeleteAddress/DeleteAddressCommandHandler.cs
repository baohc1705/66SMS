using _66SMS.Application.DTOs.Address;
using _66SMS.Domain.Abstractions.Repositories.Sql;
using MediatR;

using _66SMS.Contracts.Shared;

namespace _66SMS.Application.Features.Users.Commands.DeleteAddress
{
    public sealed record DeleteAddressCommand(Guid UserId, Guid AddressId) : IRequest<Result<bool>>;

    public sealed class DeleteAddressCommandHandler : IRequestHandler<DeleteAddressCommand, Result<bool>>
    {
        private readonly IAddressRepository addressRepo;

        public DeleteAddressCommandHandler(IAddressRepository addressRepo)
        {
            this.addressRepo = addressRepo;
        }

        public async Task<Result<bool>> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
        {
            var deleted = await addressRepo.DeleteAddressAsync(
                request.UserId,
                request.AddressId,
                cancellationToken);
                
            if (!deleted)
                return Result<bool>.NotFound("Address not found or you don't have permission to delete it.");
                
            return Result<bool>.Success(true, "Address deleted successfully.");
        }
    }
}
