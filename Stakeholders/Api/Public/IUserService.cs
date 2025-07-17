using Stakeholders.Api.Dtos;
using Stakeholders.Core.Domain;
using Stakeholders.Core.UseCases;

namespace Stakeholders.Api.Public;

public interface IUserService
{
    PagedResult<UserDto> GetPaged(int page, int pageSize);
}