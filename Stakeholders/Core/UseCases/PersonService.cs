using AutoMapper;
using FluentResults;
using Stakeholders.Api.Dtos;
using Stakeholders.Api.Public;
using Stakeholders.Constants;
using Stakeholders.Core.Domain.RepositoryInterfaces;

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
    }
}
