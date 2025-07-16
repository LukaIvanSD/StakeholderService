using Microsoft.EntityFrameworkCore;
using Stakeholders.Core.Domain;
using Stakeholders.Core.Domain.RepositoryInterfaces;

namespace Stakeholders.Infrastructure.Repositories
{
    public class UserDatabaseRepository : IUserRepository
    {
        private readonly StakeholdersContext _context;

        public UserDatabaseRepository(StakeholdersContext context)
        {
            _context = context;
        }

        public User Create(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
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
