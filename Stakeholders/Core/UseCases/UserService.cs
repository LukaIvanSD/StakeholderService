using AutoMapper;
using FluentResults;
using Stakeholders.Api.Dtos;
using Stakeholders.Api.Public;
using Stakeholders.Constants;
using Stakeholders.Core.Domain;
using Stakeholders.Core.Domain.RepositoryInterfaces;

namespace Stakeholders.Core.UseCases;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public Result<PagedResult<UserDto>> GetPaged(int page, int pageSize)
    {
        var pagedUsers = _userRepository.GetPaged(page, pageSize);
        var userDtos = _mapper.Map<List<UserDto>>(pagedUsers.Results);
        var dtoResult = new PagedResult<UserDto>(userDtos, pagedUsers.TotalCount, pagedUsers.RemainingCount);

        return Result.Ok(dtoResult);
    }

    public Result UpdateIsUserBlocked(long userId)
    {
        try
        {
            var user = _userRepository.Get(userId);

            if (user.Role == Core.Domain.UserRole.Administrator)
                return Result.Fail(FailureCode.Forbidden);

            user.ChangeIsBlocked();
            _userRepository.Update(user);

            return Result.Ok();
        }
        catch (KeyNotFoundException)
        {
            return Result.Fail(FailureCode.NotFound);
        }
    }
}