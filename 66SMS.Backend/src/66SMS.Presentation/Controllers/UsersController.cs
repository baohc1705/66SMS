using _66SMS.Application.Features.Users.Commands.AddOrUpdateAddress;
using _66SMS.Application.Features.Users.Commands.ChangePassword;
using _66SMS.Application.Features.Users.Commands.ConfirmEmail;
using _66SMS.Application.Features.Users.Commands.DeleteAddress;
using _66SMS.Application.Features.Users.Commands.ForgotPassword;
using _66SMS.Application.Features.Users.Commands.Login;
using _66SMS.Application.Features.Users.Commands.Register;
using _66SMS.Application.Features.Users.Commands.RefreshToken;
using _66SMS.Application.Features.Users.Commands.ResetPassword;
using _66SMS.Application.Features.Users.Commands.RevokeToken;
using _66SMS.Application.Features.Users.Commands.UpdateProfile;
using _66SMS.Application.Features.Users.Queries.GetAddressById;
using _66SMS.Application.Features.Users.Queries.GetAddresses;
using _66SMS.Application.Features.Users.Queries.GetProfile;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _66SMS.Presentation.Controllers
{
    /// <summary>
    /// API Controller for user authentication, profile management, and addresses.
    /// </summary>
    [ApiVersion("1.0")]
    public class UsersController : ApiController<UsersController>
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>Đăng ký tài khoản mới.</summary>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command)
        {
            var result = await _mediator.Send(command);
            return HandleResult(result);
        }

        /// <summary>Đăng nhập bằng email/username + password.</summary>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var userAgent = Request.Headers["User-Agent"].ToString();
 
            var result = await _mediator.Send(new LoginCommand(
                request.EmailOrUserName, 
                request.Password, 
                ipAddress, 
                userAgent));
            return HandleResult(result);
        }
 
        public record LoginRequest(string EmailOrUserName, string Password);

        /// <summary>Làm mới access token bằng refresh token.</summary>
        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var userAgent = Request.Headers["User-Agent"].ToString();
 
            var response = await _mediator.Send(new RefreshTokenCommand(request.RefreshToken, ipAddress, userAgent));
            return HandleResult(response);
        }
 
        public record RefreshTokenRequest(string RefreshToken);

        /// <summary>Thu hồi refresh token (đăng xuất).</summary>
        [HttpPost("revoke-token")]
        [Authorize]
        public async Task<IActionResult> RevokeToken([FromBody] string refreshToken)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var result = await _mediator.Send(new RevokeTokenCommand(refreshToken, ipAddress));
            return HandleResult(result);
        }

        /// <summary>Đổi mật khẩu (yêu cầu đăng nhập).</summary>
        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();
 
            var result = await _mediator.Send(new ChangePasswordCommand(
                userId.Value, 
                request.CurrentPassword, 
                request.NewPassword));
            return HandleResult(result);
        }
 
        public record ChangePasswordRequest(string CurrentPassword, string NewPassword);

        /// <summary>Gửi email reset mật khẩu.</summary>
        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var result = await _mediator.Send(new ForgotPasswordCommand(request.Email));
            return HandleResult(result);
        }
 
        public record ForgotPasswordRequest(string Email);

        /// <summary>Đặt lại mật khẩu bằng token từ email.</summary>
        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
        {
            var result = await _mediator.Send(command);
            return HandleResult(result);
        }


        /// <summary>Xác nhận email bằng token.</summary>
        [HttpPost("confirm-email")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailCommand command)
        {
            var result = await _mediator.Send(command);
            return HandleResult(result);
        }

        /// <summary>Lấy thông tin profile của user đang đăng nhập.</summary>
        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var result = await _mediator.Send(new GetProfileQuery(userId.Value));
            return HandleResult(result);
        }

        /// <summary>Cập nhật thông tin profile.</summary>
        [HttpPut("profile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
        {
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();
 
            var result = await _mediator.Send(new UpdateProfileCommand(
                userId.Value, 
                request.FullName, 
                request.PhoneNumber, 
                request.ProfilePhotoUrl));
            return HandleResult(result);
        }
 
        public record UpdateProfileRequest(string FullName, string PhoneNumber, string? ProfilePhotoUrl);


        /// <summary>Lấy danh sách địa chỉ của user đang đăng nhập.</summary>
        [HttpGet("addresses")]
        [Authorize]
        public async Task<IActionResult> GetAddresses()
        {
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var result = await _mediator.Send(new GetAddressesQuery(userId.Value));
            return HandleResult(result);
        }

        /// <summary>Lấy chi tiết một địa chỉ.</summary>
        [HttpGet("addresses/{addressId:guid}")]
        [Authorize]
        public async Task<IActionResult> GetAddressById(Guid addressId)
        {
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var result = await _mediator.Send(new GetAddressByIdQuery(userId.Value, addressId));
            return HandleResult(result);
        }

        /// <summary>Thêm mới hoặc cập nhật địa chỉ.</summary>
        [HttpPost("addresses")]
        [Authorize]
        public async Task<IActionResult> AddOrUpdateAddress([FromBody] AddOrUpdateAddressRequest request)
        {
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();
 
            var result = await _mediator.Send(new AddOrUpdateAddressCommand(
                request.Id, 
                userId.Value, 
                request.City, 
                request.Ward, 
                request.Street, 
                request.IsDefaultShipping, 
                request.IsDefaultBilling));
            return HandleResult(result);
        }
 
        public record AddOrUpdateAddressRequest(
            Guid? Id, 
            string City, 
            string Ward, 
            string Street, 
            bool IsDefaultShipping, 
            bool IsDefaultBilling);

        /// <summary>Xóa địa chỉ.</summary>
        [HttpDelete("addresses/{addressId:guid}")]
        [Authorize]
        public async Task<IActionResult> DeleteAddress(Guid addressId)
        {
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var result = await _mediator.Send(new DeleteAddressCommand(userId.Value, addressId));
            return HandleResult(result);
        }

        private Guid? GetCurrentUserId()
        {
            var value = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(value, out var id) ? id : null;
        }
    }
}
