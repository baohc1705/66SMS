using System.ComponentModel.DataAnnotations;

namespace _66SMS.Application.DTOs.Identity.Commands
{
    public class ConfirmEmailDTO
    {
        [Required(ErrorMessage = "User ID is required.")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Confirmation token is required.")]
        public string Token { get; set; } = null!;
    }
}
