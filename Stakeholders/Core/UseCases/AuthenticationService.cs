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
    public class AuthenticationService :IAuthenticationService
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly ITokenGenerator tokenGenerator;
        private readonly IPersonRepository personRepository;
        private readonly FollowerClient followerClient;

        public AuthenticationService(IUserRepository _userRepository, IMapper _mapper, ITokenGenerator _tokenGenerator,IPersonRepository _personRepository, FollowerClient _followerClient)
        {
            userRepository = _userRepository;
            mapper = _mapper;
            tokenGenerator = _tokenGenerator;
            personRepository = _personRepository;
            followerClient = _followerClient;
        }

        public Result<TokenDto> GetToken(string token)
        {
            return tokenGenerator.GetToken(token);
        }

        public Result<AuthenticationTokenDto> Login(CredentialsDto credentialsDto)
        {
            var user = userRepository.GetByEmail(credentialsDto.Email);
            if (user == null || credentialsDto.Password != user.Password) return Result.Fail(FailureCode.NotFound);
            var person = personRepository.GetByUserId(user.Id);
            return tokenGenerator.GenerateToken(user,person.Id);
        }
        public async Task<Result<AuthenticationTokenDto>> Register(AccountRegistrationDto accountDto)
        {
            var userToRegister = new User(
                accountDto.Username,
                accountDto.Password,
                accountDto.Email,
                Enum.Parse<Core.Domain.UserRole>(accountDto.Role),
                false
            );

            if (userToRegister.IsAdmin())
                return Result.Fail(FailureCode.InvalidArgument);

            if (userRepository.Exists(userToRegister.Email))
                return Result.Fail(FailureCode.NonUniqueEmail);

            long userId = 0;
            long personId = 0;

            try
            {
                var user = userRepository.Create(userToRegister);
                userId = user.Id;

                var person = personRepository.Create(new Person(
                    user.Id,
                    accountDto.Name,
                    accountDto.Surname,
                    accountDto.PictureBase64,
                    accountDto.Bio,
                    accountDto.Moto
                ));
                personId = person.Id;

                var followerRegistered = await followerClient.AddUserAsync(userId);

                if (!followerRegistered)
                {
                    Console.WriteLine("[SAGA] Follower service returned failure. Triggering compensation...");
                    await CompensateUserCreationAsync(userId, personId);
                    return Result.Fail(new Error("Follower service failed").WithMetadata("reason", "external_service"));
                }

                return tokenGenerator.GenerateToken(user, person.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SAGA] Exception during registration: {ex.Message}");

                if (userId > 0)
                {
                    await CompensateUserCreationAsync(userId, personId);
                }

                return Result.Fail(FailureCode.Conflict);
            }


        }

        private async Task CompensateUserCreationAsync(long userId, long personId)
        {
            try
            {
                if (personId > 0)
                {
                    personRepository.Delete(personId);
                    Console.WriteLine($"[SAGA] Rolled back person with ID={personId}");
                }

                if (userId > 0)
                {
                    userRepository.Delete(userId);
                    Console.WriteLine($"[SAGA] Rolled back user with ID={userId}");
                }

                await Task.CompletedTask; 
                Console.WriteLine($"[SAGA] Compensation completed successfully for userId={userId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SAGA] Compensation failed for userId={userId}: {ex.Message}");
            }
        }
    }
}
