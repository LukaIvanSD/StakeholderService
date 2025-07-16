using FluentResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Stakeholders.Constants;
using Stakeholders.Core.UseCases;

namespace Stakeholders.Infrastructure
{
    public class StakeholdersUnitOfWork : IUnitOfWork
    {
        private readonly StakeholdersContext _context;
        private IDbContextTransaction? _transaction;

        public StakeholdersUnitOfWork(StakeholdersContext context)
        {
            _context = context;
        }

        public void BeginTransaction()
        {
            _transaction ??= _context.Database.BeginTransaction();
        }

        public Result Save()
        {
            try
            {
                _context.SaveChanges();
                return Result.Ok();
            }
            catch (DbUpdateException e)
            {
                var rootError = new Error(e.Message).CausedBy(e);
                return Result.Fail(FailureCode.Conflict).WithError(rootError);
            }
        }

        public Result Commit()
        {
            if (_transaction is null) return Result.Fail("No active transaction.");

            _transaction.Commit();
            _transaction.Dispose();
            _transaction = null;
            return Result.Ok();
        }

        public Result Rollback()
        {
            if (_transaction is null) return Result.Fail("No active transaction.");

            _transaction.Rollback();
            _transaction.Dispose();
            _transaction = null;
            return Result.Ok();
        }
    }
}
