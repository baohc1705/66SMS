using System.ComponentModel.DataAnnotations;

namespace _66SMS.Application.DTOs.Identity.Commands
{
    public class RefreshTokenRequestDTO
    {
        [Required(ErrorMessage = "Refresh token is required.")]
        public string RefreshToken { get; set; } = null!;
    }
}
