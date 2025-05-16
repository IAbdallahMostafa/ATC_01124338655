using Booking.Core.DTOs.Authentication;
using Booking.Core.Intefaces;
using Microsoft.AspNetCore.Mvc;

namespace Booking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthenticationRepositry _authenticationRepositry;
        public AccountController(IAuthenticationRepositry authenticationRepositry)
        {
            _authenticationRepositry = authenticationRepositry;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            if (ModelState.IsValid)
            {
                var authModel = await _authenticationRepositry.Register(registerDTO);
                return Ok(authModel);
            }
            return BadRequest(ModelState);
        }

        [HttpGet("GetNewRefreshToken")]
        public async Task<IActionResult> GetNewRefreshToken(string currentRefreshToke)
        {
            if (ModelState.IsValid)
            {
                var result = await _authenticationRepositry.GetNewRefreshToken(currentRefreshToke);
                if (!result.isAuthenticated)
                    return BadRequest(result.Message);

                return Ok(result);
            }
            return BadRequest(currentRefreshToke);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            if (ModelState.IsValid)
            {
                var result = await _authenticationRepositry.Login(loginDTO);
                if (!result.isAuthenticated)
                    return BadRequest(result.Message);

                return Ok(result);
            }
            return BadRequest(ModelState);
        }

        [HttpPost("Revoke")]
        public async Task<IActionResult> RevokeRefreshToken(string refreshToken)
        {
            if (ModelState.IsValid)
            {
                var result = await _authenticationRepositry.RevokeRefreshToken(refreshToken);
                if (!string.IsNullOrEmpty(result))
                    return BadRequest(result);
                return Ok("Token Revoked Successfully");
            }
            return BadRequest(ModelState);
        }

    }
}
