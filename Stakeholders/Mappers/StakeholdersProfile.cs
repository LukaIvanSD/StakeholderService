using AutoMapper;
using Stakeholders.Api.Dtos;
using Stakeholders.Core.Domain;
using Auth;

namespace Stakeholders.Mappers
{
    public class StakeholdersProfile : Profile
    {
        public StakeholdersProfile()
        {
            CreateMap<AccountRegistrationDto, User>();
            CreateMap<PersonDto, Person>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<CredentialsDto, CredentialsRequest>().ReverseMap();
            CreateMap<AuthenticationTokenResponse, AuthenticationTokenDto>().ReverseMap();
            CreateMap<AccountRegistrationDto, AccountRegistrationRequest>().ReverseMap();
            CreateMap<PersonResponse, PersonDto>().ReverseMap();
            CreateMap<UsersPagedResponse, Core.UseCases.PagedResult<UserDto>>().ReverseMap();
            CreateMap<UserResponse, UserDto>().ReverseMap();
        }
    }
}
