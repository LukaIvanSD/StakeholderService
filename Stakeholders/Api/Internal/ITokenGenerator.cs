using FluentResults;
using Stakeholders.Api.Dtos;
using Stakeholders.Core.Domain;
using Microsoft.IdentityModel.Tokens;

namespace Stakeholders.Api.Internal
{
    public interface ITokenGenerator
    {
        public Result<AuthenticationTokenDto> GenerateToken(User user,long personId);
        public Result<TokenDto> GetToken(string token);
  }
}
