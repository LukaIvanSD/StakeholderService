using Microsoft.EntityFrameworkCore;
using Stakeholders.Core.Domain;
using Stakeholders.Core.Domain.RepositoryInterfaces;
using Stakeholders.Core.UseCases;

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
        
        public PagedResult<User> GetPaged(int page, int pageSize)
        {
            var totalCount = _context.Users.Count();
            var users = _context.Users
                .OrderBy(u => u.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var remainingCount = Math.Max(0, totalCount - page * pageSize);
            return new PagedResult<User>(users, totalCount, remainingCount);
        }
    }
}
