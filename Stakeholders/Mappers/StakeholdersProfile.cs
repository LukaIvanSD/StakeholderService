using AutoMapper;
using Stakeholders.Api.Dtos;
using Stakeholders.Core.Domain;

namespace Stakeholders.Mappers
{
    public class StakeholdersProfile : Profile
    {
        public StakeholdersProfile()
        {
            CreateMap<AccountRegistrationDto, User>();
            CreateMap<PersonDto, Person>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
