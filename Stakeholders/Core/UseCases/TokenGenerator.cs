using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FluentResults;
using Microsoft.IdentityModel.Tokens;
using Stakeholders.Api.Dtos;
using Stakeholders.Api.Internal;
using Stakeholders.Core.Domain;

namespace Stakeholders.Core.UseCases
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly string _key = Environment.GetEnvironmentVariable("JWT_KEY") ?? "ultra_extra_long_super_secret_soa_key";
        private readonly string _issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "soa";
        private readonly string _audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "soa-front.com";

        public Result<AuthenticationTokenDto> GenerateToken(User user,long personId)
        {
            var authenticationResponse = new AuthenticationTokenDto();

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new("id", user.Id.ToString()),
                new("personId",personId.ToString()),
                new(ClaimTypes.Role, user.GetPrimaryRoleName())
            };

            var jwt = CreateToken(claims, 60 * 24);
            authenticationResponse.Id = user.Id;
            authenticationResponse.AccesToken = jwt;

            return authenticationResponse;
        }

        private string CreateToken(IEnumerable<Claim> claims, double expirationTimeInMinutes)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _issuer,
                _audience,
                claims,
                expires: DateTime.Now.AddMinutes(expirationTimeInMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
