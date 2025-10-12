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
        private readonly IUnitOfWork unitOfWork;
        private readonly FollowerClient followerClient;

        public AuthenticationService(IUserRepository _userRepository, IMapper _mapper, ITokenGenerator _tokenGenerator,IPersonRepository _personRepository,IUnitOfWork _unitOfWork, FollowerClient _followerClient)
        {
            userRepository = _userRepository;
            mapper = _mapper; 
            tokenGenerator = _tokenGenerator;
            personRepository = _personRepository;
            unitOfWork = _unitOfWork;
            followerClient = _followerClient;
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
                Enum.Parse<UserRole>(accountDto.Role)
            );

            if (userToRegister.IsAdmin())
                return Result.Fail(FailureCode.InvalidArgument);

            if (userRepository.Exists(userToRegister.Email))
                return Result.Fail(FailureCode.NonUniqueEmail);


            try
            {
                long userId = 0;
                long personId = 0;
                unitOfWork.BeginTransaction();

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

                unitOfWork.Commit();


                try
                {
                    bool followerRegistered = false;
                    followerRegistered = await followerClient.AddUserAsync(userId);

                    if (!followerRegistered)
                    {
                        Console.WriteLine("[SAGA] Follower service returned failure. Triggering compensation.");
                        await CompensateUserCreationAsync(userId, personId);
                        return Result.Fail(new Error("Follower service failed").WithMetadata("reason", "external_service"));

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[SAGA] Exception during Follower service call: {ex.Message}");
                    await CompensateUserCreationAsync(userId, personId);
                    return Result.Fail(new Error("Follower service failed").WithMetadata("reason", "external_service"));

                }

                return tokenGenerator.GenerateToken(user, person.Id);
            }
            catch (Exception ex)
            {
                try
                {
                    unitOfWork.Rollback();
                }
                catch (Exception rollbackEx)
                {
                    Console.WriteLine($"[ERROR] Rollback failed: {rollbackEx.Message}");
                }

                Console.WriteLine($"[ERROR] Registration failed: {ex.Message}");
                return Result.Fail(FailureCode.Conflict);
            }
        }

        private async Task CompensateUserCreationAsync(long userId, long personId)
        {
            try
            {
                unitOfWork.BeginTransaction();
                personRepository.Delete(personId);
                userRepository.Delete(userId);
                unitOfWork.Commit();

                Console.WriteLine($"[SAGA] Compensation executed successfully for userId={userId}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SAGA] Compensation failed for userId={userId}: {ex.Message}");
                await Task.CompletedTask;
            }
        }
    }
}
