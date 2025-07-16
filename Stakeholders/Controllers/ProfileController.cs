using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stakeholders.Api.Dtos;
using Stakeholders.Api.Public;
using Stakeholders.Infrastructure.Authentication;

namespace Stakeholders.Controllers
{
    [ApiController]
    [Route("api/profile")]
    [Authorize("authorOrTouristPolicy")]
    public class ProfileController : BaseApiController
    {
        private readonly IPersonSerivce personService;

        public ProfileController(IPersonSerivce _personService)
        {
            personService = _personService;
        }

        [HttpGet]
        public ActionResult<PersonDto> GetProfile()
        {
            var result = personService.GetById(User.PersonId());
            return CreateResponse(result);
        }

    }
}
