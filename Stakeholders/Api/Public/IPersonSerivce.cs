using FluentResults;
using Stakeholders.Api.Dtos;

namespace Stakeholders.Api.Public
{
    public interface IPersonSerivce
    {
        public Result<PersonDto> GetById(long id);
    }
}
