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

namespace Stakeholders.Core.UseCases
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly ITokenGenerator tokenGenerator;
        private readonly IPersonRepository personRepository;

        public AuthenticationService(IUserRepository _userRepository, IMapper _mapper, ITokenGenerator _tokenGenerator,IPersonRepository _personRepository)
        {
            userRepository = _userRepository;
            mapper = _mapper; 
            tokenGenerator = _tokenGenerator;
            personRepository = _personRepository;
        }

        public Result<AuthenticationTokenDto> Login(CredentialsDto credentialsDto)
        {
            var user = userRepository.GetByEmail(credentialsDto.Email);
            if (user == null || credentialsDto.Password != user.Password) return Result.Fail(FailureCode.NotFound);
            var person = personRepository.GetByUserId(user.Id);
            return tokenGenerator.GenerateToken(user,person.Id);
        }

        public Result<AuthenticationTokenDto> Register(AccountRegistrationDto accountDto)
        {
            User userToRegister = new User(accountDto.Username, accountDto.Password, accountDto.Email,
                Enum.Parse<UserRole>(accountDto.Role), false);
            if (userToRegister.IsAdmin()) return Result.Fail(FailureCode.InvalidArgument);
            if (userRepository.Exists(userToRegister.Email)) return Result.Fail(FailureCode.NonUniqueEmail);
            try
            {
                var user = userRepository.Create(userToRegister);
                var person = personRepository.Create(new Person(user.Id, accountDto.Name, accountDto.Surname,
                    accountDto.PictureBase64, accountDto.Bio, accountDto.Moto));
                return tokenGenerator.GenerateToken(user, person.Id);
            }
            catch (Exception ex)
            {
                return Result.Fail(FailureCode.Conflict);
            }
        }
    }
}
