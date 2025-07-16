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
        private readonly IPersonRepository personRepository;
        private readonly IMapper mapper;

        public PersonService(IPersonRepository _personRepository,IMapper _mapper)
        {
            personRepository = _personRepository;
            mapper = _mapper;
        }
        public Result<PersonDto> GetById(long id)
        {
            var person = personRepository.GetByUserId(id);
            if (person == null) return Result.Fail(FailureCode.NotFound);
            return mapper.Map<PersonDto>(person);
        }
    }
}
