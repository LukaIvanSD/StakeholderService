using Stakeholders.Core.UseCases;

namespace Stakeholders.Core.Domain.RepositoryInterfaces
{
    public interface IUserRepository : ICrudRepository<User>
    {
        public User Get(long id);
        public User Create(User user);
        public bool Exists(string email);
        public User? GetByEmail(string email);
    }
}
