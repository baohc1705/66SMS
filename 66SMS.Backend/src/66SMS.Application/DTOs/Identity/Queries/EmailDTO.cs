using System.ComponentModel.DataAnnotations;

namespace _66SMS.Application.DTOs.Identity.Queries
{
    public class EmailDTO
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; } = null!;
    }
}
