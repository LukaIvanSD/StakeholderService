using FluentResults;
using Stakeholders.Api.Dtos;
using Stakeholders.Core.Domain;

namespace Stakeholders.Api.Internal
{
    public interface ITokenGenerator
    {
        public Result<AuthenticationTokenDto> GenerateToken(User user,long personId);
    }
}
