using AutoMapper;
using FluentResults;
using Stakeholders.Api.Dtos;
using Stakeholders.Api.Internal;
using Stakeholders.Api.Public;
using Stakeholders.Constants;
using Stakeholders.Core.Domain;
using Stakeholders.Core.Domain.RepositoryInterfaces;
using System;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using Grpc.Core;
using Stakeholders;
using Microsoft.AspNetCore.Authorization;
namespace Stakeholders.GrpcsServices
{
    [Authorize("administratorPolicy")]
    public class UserGrpcService : UserService.UserServiceBase
  {
    private readonly IMapper _mapper;
    private readonly IUserService userService;
    public UserGrpcService(IMapper mapper, IUserService userService)
    {
      _mapper = mapper;
      this.userService = userService;
    }

    public override Task<UsersPagedResponse> GetPaged(PagedRequest pagedRequest, ServerCallContext context)
    {
      var result = userService.GetPaged(pagedRequest.Page, pagedRequest.PageSize);
      return Task.FromResult(_mapper.Map<UsersPagedResponse>(result.Value));
    }

    public override Task<ToggleBlockResponse> ToggleBlock(ToggleBlockRequest toggleBlockRequst, ServerCallContext context)
    {
      var result = userService.UpdateIsUserBlocked(toggleBlockRequst.Id);
      return Task.FromResult(new ToggleBlockResponse() { Success = result == Result.Ok() });
    }
  }
}
