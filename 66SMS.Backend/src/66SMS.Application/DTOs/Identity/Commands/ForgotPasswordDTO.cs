using System.ComponentModel.DataAnnotations;

namespace _66SMS.Application.DTOs.Identity.Commands
{
    /// <summary>
    /// Input DTO để trigger forgot password flow (chứa email cần reset).
    /// ForgotPasswordResponseDTO (kết quả trả về) nằm trong Queries.
    /// </summary>
    public class ForgotPasswordDTO
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; } = null!;
    }
}
