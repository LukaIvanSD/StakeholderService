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
using Auth;

namespace Stakeholders.GrpcsServices
{
    public class AuthenticationGrpcService :AuthService.AuthServiceBase
    {
        private readonly IAuthenticationService authenticationService;
      private readonly IMapper _mapper;
    public AuthenticationGrpcService(IAuthenticationService authenticationService, IMapper mapper)
    {
      this.authenticationService = authenticationService;
      _mapper = mapper;
    }
    public override Task<TokenResponse> GetToken(EmptyMessage message, ServerCallContext context)
    {
      Console.WriteLine("aasda");
       var authHeader = context.RequestHeaders
        .FirstOrDefault(h => h.Key.Equals("authorization", StringComparison.OrdinalIgnoreCase));

        string? token = null;

        if (authHeader != null && authHeader.Value.StartsWith("Bearer "))
        {
            token = authHeader.Value.Substring("Bearer ".Length);
        }
          var result = authenticationService.GetToken(token);
          Console.WriteLine(result.Value.IsValid);
          Console.WriteLine(result.Value.Role);
          return Task.FromResult(_mapper.Map<TokenResponse>(result.Value));
        }

        public override Task<AuthenticationTokenResponse> Login(CredentialsRequest credentialsRequest,ServerCallContext context)
        {
            var result = authenticationService.Login(_mapper.Map<CredentialsDto>(credentialsRequest));
            return Task.FromResult(_mapper.Map<AuthenticationTokenResponse>(result.Value));
        }

        public override Task<AuthenticationTokenResponse> Register(AccountRegistrationRequest accountDto,ServerCallContext context)
        {
            var result = authenticationService.Register(_mapper.Map<AccountRegistrationDto>(accountDto));
            return Task.FromResult(_mapper.Map<AuthenticationTokenResponse>(result.Value));
        }
    }
}
