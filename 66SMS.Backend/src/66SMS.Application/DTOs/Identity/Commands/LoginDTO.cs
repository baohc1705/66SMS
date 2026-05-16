using System.ComponentModel.DataAnnotations;

namespace _66SMS.Application.DTOs.Identity.Commands
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Email or Username is required.")]
        public string EmailOrUserName { get; set; } = null!;
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; } = null!;
    }
}
