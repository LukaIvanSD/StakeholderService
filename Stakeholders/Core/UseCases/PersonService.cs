using AutoMapper;
using FluentResults;
using Stakeholders.Api.Dtos;
using Stakeholders.Api.Public;
using Stakeholders.Constants;
using Stakeholders.Core.Domain.RepositoryInterfaces;
using Stakeholders.Core.Domain;

namespace Stakeholders.Core.UseCases
{
    public class PersonService : IPersonSerivce
    {
        private readonly IPersonRepository _personRepository;
        private readonly IMapper _mapper;

        public PersonService(IPersonRepository _personRepository,IMapper _mapper)
        {
            this._personRepository = _personRepository;
            this._mapper = _mapper;
        }
        public Result<PersonDto> GetById(long id)
        {
            var person = _personRepository.GetByUserId(id);
            if (person == null) return Result.Fail(FailureCode.NotFound);
            return _mapper.Map<PersonDto>(person);
        }

        public Result UpdateUserProfile(PersonDto userInfo, long userId)
        {
            var person = _personRepository.GetByUserId(userId);
            if (person == null)
                return Result.Fail(FailureCode.NotFound);

            try
            {
                person.UpdateProfile(
                    userInfo.Name,
                    userInfo.Surname,
                    userInfo.PictureBase64 ?? string.Empty,
                    userInfo.Bio ?? string.Empty,
                    userInfo.Moto ?? string.Empty
                );

                _personRepository.Update(person);

                return Result.Ok();
            }
            catch (ArgumentException ex)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError(ex.Message);
            }
        }
    }
}
