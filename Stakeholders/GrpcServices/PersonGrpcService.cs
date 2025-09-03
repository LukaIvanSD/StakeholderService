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

namespace Stakeholders.GrpcsServices
{
    public class PersonGrpcService : PersonService.PersonServiceBase
  {
    private readonly IMapper _mapper;
    private readonly IPersonSerivce personService;
    public PersonGrpcService(IMapper mapper, IPersonSerivce personService)
    {
      _mapper = mapper;
      this.personService = personService;

    }
    public override Task<PersonResponse> GetProfile(EmptyRequest message, ServerCallContext context)
    {
      var user = context.GetHttpContext().User;
      var userId = long.Parse(user.Claims.First(c => c.Type == "id").Value);
      var result = personService.GetById(userId);
      return Task.FromResult(_mapper.Map<PersonResponse>(result.Value));
    }
  }
}
