namespace Stakeholders.Core.Domain.RepositoryInterfaces
{
    public interface IPersonRepository
    {
        public Person? GetByUserId(long userId);
        public Person? GetById(long id);
        public Person Create(Person person);
        void Delete(long personId);
    }
}
