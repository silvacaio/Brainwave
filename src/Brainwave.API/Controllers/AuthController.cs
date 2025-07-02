using Brainwave.API.Controllers.Base;
using Brainwave.API.Extensions;
using Brainwave.API.ViewModel;
using Brainwave.Core.Messages.CommonMessages.Notifications;
using Brainwave.ManagementStudents.Application.Commands.User;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Brainwave.API.Controllers
{
    [Route("api/account")]
    public class AuthenticationController : MainController
    {
        private readonly IMediator _mediator;
        private readonly SignInManager<IdentityUser<Guid>> _signInManager;
        private readonly UserManager<IdentityUser<Guid>> _userManager;
        private readonly JwtSettings _jwtSettings;

        public AuthenticationController(
            INotificationHandler<DomainNotification> notifications,
            IMediator mediator,
            SignInManager<IdentityUser<Guid>> signInManager,
            UserManager<IdentityUser<Guid>> userManager,
            JwtSettings jwtSettings)
            : base(notifications, mediator)
        {
            _mediator = mediator;
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtSettings = jwtSettings;
        }

        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        [HttpPost("register/student")]
        public async Task<ActionResult> RegisterStudent(RegisterUserViewModel registerUser)
        {
            if (!ModelState.IsValid)
            {
                NotifyError(ModelState);
                return CustomResponse();
            }

            var result = await RegisterUser(registerUser, "STUDENT");

            if (!result.IdentityResult.Succeeded)
            {
                NotifyError(result.IdentityResult);
                return CustomResponse();
            }

            var command = new AddStudentCommand(result.UserId, registerUser.Name);
            await _mediator.Send(command);

            if (!IsOperationValid())
                return CustomResponse();

            var token = await GetJwt(registerUser.Email!);
            return CustomResponse(token);
        }

        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        [HttpPost("register/admin")]
        public async Task<ActionResult> RegisterAdmin(RegisterUserViewModel registerUser)
        {
            if (!ModelState.IsValid)
            {
                NotifyError(ModelState);
                return CustomResponse();
            }

            var result = await RegisterUser(registerUser, "ADMIN");

            if (!result.IdentityResult.Succeeded)
            {
                NotifyError(result.IdentityResult);
                return CustomResponse();
            }

            var command = new AddAdminCommand(result.UserId, registerUser.Name);
            await _mediator.Send(command);

            if (!IsOperationValid())
                return CustomResponse();

            var token = await GetJwt(registerUser.Email!);
            return CustomResponse(token);
        }

        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginUserViewModel loginUser)
        {
            if (!ModelState.IsValid)
            {
                NotifyError(ModelState);
                return ValidationProblem();
            }

            var result = await _signInManager.PasswordSignInAsync(loginUser.Email!, loginUser.Password!, false, true);

            if (result.Succeeded)
                return CustomResponse(await GetJwt(loginUser.Email!));

            if (result.IsLockedOut)
            {
                NotifyError("Identity", "User temporarily locked. Please try again later.");
                return CustomResponse(loginUser);
            }

            NotifyError("Identity", "Invalid username or password.");
            return CustomResponse(loginUser);
        }

        private async Task<(IdentityResult IdentityResult, Guid UserId)> RegisterUser(RegisterUserViewModel registerUser, string role)
        {
            var userIdentity = new IdentityUser<Guid>
            {
                UserName = registerUser.Name,
                Email = registerUser.Email,
                EmailConfirmed = true,
            };

            var result = await _userManager.CreateAsync(userIdentity, registerUser.Password!);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(userIdentity, role);
            }

            return (result, result.Succeeded ? userIdentity.Id : Guid.Empty);
        }

        private async Task<LoginResponseViewModel> GetJwt(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            var claims = await BuildUserClaims(user);
            var token = GenerateJwtToken(claims);
            return BuildLoginResponse(token, user, claims);
        }

        private async Task<List<Claim>> BuildUserClaims(IdentityUser<Guid> user)
        {
            var claims = (await _userManager.GetClaimsAsync(user)).ToList();

            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));

            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            return claims;
        }

        private string GenerateJwtToken(List<Claim> claims)
        {
            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);
            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = credentials,
                Subject = identityClaims,
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpirationHours)
            };

            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(tokenDescriptor);

            return handler.WriteToken(token);
        }

        private LoginResponseViewModel BuildLoginResponse(string token, IdentityUser<Guid> user, List<Claim> claims)
        {
            return new LoginResponseViewModel
            {
                AccessToken = token,
                ExpiresIn = TimeSpan.FromHours(_jwtSettings.ExpirationHours).TotalSeconds,
                UserToken = new UserTokenViewModel
                {
                    Id = user.Id.ToString(),
                    Email = user.Email,
                    Claims = claims.Select(c => new ClaimViewModel { Type = c.Type, Value = c.Value })
                }
            };
        }


        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

    }
}