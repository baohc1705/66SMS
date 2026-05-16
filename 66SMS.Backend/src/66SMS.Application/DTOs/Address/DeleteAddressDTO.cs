using System.ComponentModel.DataAnnotations;

namespace _66SMS.Application.DTOs.Address
{
    public class DeleteAddressDTO
    {
        [Required(ErrorMessage = "User ID is required.")]
        public Guid UserId { get; set; }
        [Required(ErrorMessage = "Address ID is required.")]
        public Guid AddressId { get; set; }
    }
}
