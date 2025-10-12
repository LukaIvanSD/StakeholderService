
using Microsoft.AspNetCore.Mvc;
using Stakeholders.Api.Dtos;
using Stakeholders.Api.Public;

namespace Stakeholders.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController : BaseApiController
    {
        private readonly IAuthenticationService authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
                this.authenticationService = authenticationService;
        }

        [HttpPost]
        public async Task<ActionResult<AuthenticationTokenDto>> Register(
            [FromBody] AccountRegistrationDto accountRegistrationDto)
        {
            var result = await authenticationService.Register(accountRegistrationDto);
            return CreateResponse(result);
        }

        [HttpPost("login")]
        public ActionResult<AuthenticationTokenDto> Login([FromBody] CredentialsDto credentialsDto)
        {
            var result = authenticationService.Login(credentialsDto);
            return CreateResponse(result);
        }
    }
}
