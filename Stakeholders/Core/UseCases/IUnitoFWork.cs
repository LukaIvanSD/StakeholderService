using FluentResults;

namespace Stakeholders.Core.UseCases
{
    public interface IUnitOfWork
    {
        void BeginTransaction();
        Result Save();
        Result Commit();
        Result Rollback();
    }
}
