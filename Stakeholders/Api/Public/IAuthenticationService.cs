using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Stakeholders.Api.Dtos;

namespace Stakeholders.Api.Public
{
    public interface IAuthenticationService
    {
        public Result<AuthenticationTokenDto> Login(CredentialsDto credentialsDto);
        public Task<Result<AuthenticationTokenDto>> Register(AccountRegistrationDto accountDto);
    }
}
