using AutoMapper;
using Booking.Core.DTOs.Authentication;
using Booking.Core.Enities.Authentication;
using Booking.Core.Helpers;
using Booking.Core.Intefaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Booking.Infrasturcture.Services
{
    public class AuthenticationRepositry : IAuthenticationRepositry
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JwtSettings _jwtSettings;
        private readonly IMapper _mapper;

        public AuthenticationRepositry(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
           IOptions<JwtSettings> jwtSettings, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtSettings = jwtSettings.Value;
            _mapper = mapper;
        }

        public async Task<AuthenticationModel> Register(RegisterDTO registerDTO)
        {
            AuthenticationModel authenticationModel = new AuthenticationModel();
            
            if(await _userManager.FindByNameAsync(registerDTO.UserName) != null)
            {
                authenticationModel.Message = "Username already exists!";
                return authenticationModel;
            }

            if (await _userManager.FindByEmailAsync(registerDTO.Email) != null)
            {
                authenticationModel.Message = "Email already exists!";
                return authenticationModel;
            }

            ApplicationUser user = new ApplicationUser();
            _mapper.Map(registerDTO, user);

            var result = await _userManager.CreateAsync(user, registerDTO.Password);
            if (!result.Succeeded)
            {
                string error = string.Empty;
                foreach (var errorItem in result.Errors)
                    error += errorItem.Description + "\n";
                return authenticationModel;
            }

            await _userManager.AddToRoleAsync(user, "User");
            var accessToken = await GenerateAccessToken(user);
            var refreshToken = GenerateRefreshToken();

            user.RefreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(user);

            authenticationModel.Message = "Success";
            authenticationModel.UserName = user.UserName!;
            authenticationModel.Email = user.Email!;
            authenticationModel.isAuthenticated = true;
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                authenticationModel.Roles.Add(role);
            }
            authenticationModel.AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken);
            authenticationModel.AccessTokenExpiration = accessToken.ValidTo;
            authenticationModel.RefreshToken = refreshToken.Token;
            authenticationModel.RefreshTokenExpiration = refreshToken.ExpiresOn;

            return authenticationModel;
        }

        public async Task<AuthenticationModel> GetNewRefreshToken(string currentRefreshToken)
        {
            var authenticationModel = new AuthenticationModel();

            var user = await _userManager.Users.SingleOrDefaultAsync(e => e.RefreshTokens.Any(e => e.Token == currentRefreshToken));

            if (user is null)
            {
                authenticationModel.Message = "Invalid token!";
                return authenticationModel;
            }

            var refreshTokenInDB = user.RefreshTokens.Single(e => e.Token == currentRefreshToken);

            if (!refreshTokenInDB.IsValid)
            {
                authenticationModel.Message = "InActive token!";
                return authenticationModel;
            }

            refreshTokenInDB.RevokedOn = DateTime.UtcNow;

            var newRefreshToken = GenerateRefreshToken();

            user.RefreshTokens.Add(newRefreshToken);

            await _userManager.UpdateAsync(user);

            var newAccessToken = await GenerateAccessToken(user);

            authenticationModel.Message = "Genreate new token successed";
            authenticationModel.Email = user.Email!;
            authenticationModel.UserName = user.UserName!;
            authenticationModel.AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken);
            authenticationModel.AccessTokenExpiration = newAccessToken.ValidTo;
            authenticationModel.RefreshToken = newRefreshToken.Token;
            authenticationModel.RefreshTokenExpiration = newRefreshToken.ExpiresOn;
            authenticationModel.isAuthenticated = true;
            foreach (var role in await _userManager.GetRolesAsync(user))
                authenticationModel.Roles.Add(role);

            return authenticationModel;
        }

        public async Task<AuthenticationModel> Login(LoginDTO loginDTO)
        {
            var authenticationModel = new AuthenticationModel();

                var user = await _userManager.FindByNameAsync(loginDTO.UserName);
            if (user is null || !await _userManager.CheckPasswordAsync(user, loginDTO.Password))
            {
                authenticationModel.Message = "Invalid username or password!";
                return authenticationModel;
            }

            var newAccessToken = await GenerateAccessToken(user);

            if (user.RefreshTokens.Any(e => e.IsValid))
            {
                var activeRefreshToken = user.RefreshTokens.SingleOrDefault(e => e.IsValid);
                authenticationModel.RefreshToken = activeRefreshToken!.Token;
                authenticationModel.RefreshTokenExpiration = activeRefreshToken.ExpiresOn;
            }
            else
            {
                var newRefreshToken = GenerateRefreshToken();
                authenticationModel.RefreshToken = newRefreshToken!.Token;
                authenticationModel.RefreshTokenExpiration = newRefreshToken.ExpiresOn;
                user.RefreshTokens.Add(newRefreshToken);
                await _userManager.UpdateAsync(user);
            }
            authenticationModel.Message = "Login Successed";
            authenticationModel.isAuthenticated = true;
            authenticationModel.UserName = user.UserName!;
            authenticationModel.Email = user.Email!;
            authenticationModel.AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken);
            authenticationModel.AccessTokenExpiration = newAccessToken.ValidTo;
            var userRefreshToken = user.RefreshTokens.Single(e => e.IsValid);
            authenticationModel.RefreshToken = userRefreshToken.Token;
            authenticationModel.RefreshTokenExpiration = userRefreshToken.ExpiresOn;
            foreach (var role in await _userManager.GetRolesAsync(user))
                authenticationModel.Roles.Add(role);

            return authenticationModel;
        }

        
        public async Task<string> RevokeRefreshToken(string refreshToken)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(e => e.RefreshTokens.Any(e => e.Token == refreshToken));

            if (user is null)
                return "Invliad token!";

            var refreshTokenInDB = user.RefreshTokens.Single(e => e.Token == refreshToken);
            if (!refreshTokenInDB.IsValid)
                return "Invliad token!";

            refreshTokenInDB.RevokedOn = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);
            return string.Empty;
        }


        private async Task<JwtSecurityToken> GenerateAccessToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);
            var roleCliams = new List<Claim>();
            foreach (var role in userRoles)
                roleCliams.Add(new Claim("Roles", role));

            var allClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            }.Union(userClaims)
            .Union(roleCliams);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var signutre = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: allClaims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes),
                signingCredentials: signutre
            );
            return token;
        }

        private RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            RandomNumberGenerator.Fill(randomNumber);

            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                CreatedOn = DateTime.UtcNow,
                ExpiresOn = DateTime.UtcNow.AddDays(10)
            };

            return refreshToken;
        }


    }
}
