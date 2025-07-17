using Microsoft.EntityFrameworkCore;
using Stakeholders.Core.Domain;
using Stakeholders.Core.Domain.RepositoryInterfaces;
using Stakeholders.Core.UseCases;

namespace Stakeholders.Infrastructure.Repositories
{
    public class UserDatabaseRepository : CrudDatabaseRepository<User, StakeholdersContext>,IUserRepository
    {
        private readonly StakeholdersContext _context;

        public UserDatabaseRepository(StakeholdersContext context) :  base(context)
        {
            _context = context;
        }
        
        public bool Exists(string email)
        {
            return _context.Users.Any(user => user.Email == email);
        }

        public User? GetByEmail(string email)
        {
            return _context.Users.FirstOrDefault(user => user.Email == email);
        }
        
    }
}
