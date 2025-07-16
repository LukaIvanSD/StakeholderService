namespace Stakeholders.Core.Domain.RepositoryInterfaces
{
    public interface IUserRepository
    {
        public User Create(User user);
        public bool Exists(string email);
        public User? GetByEmail(string email);
    }
}
