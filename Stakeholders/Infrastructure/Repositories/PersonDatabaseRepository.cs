using Stakeholders.Core.Domain;
using Stakeholders.Core.Domain.RepositoryInterfaces;

namespace Stakeholders.Infrastructure.Repositories
{
    public class PersonDatabaseRepository : IPersonRepository
    {
        private readonly StakeholdersContext _context;

        public PersonDatabaseRepository(StakeholdersContext context)
        {
            _context = context;
        }

        public Person Create(Person person)
        {
            _context.Persons.Add(person);
            _context.SaveChanges();
            return person;
        }

        public Person Update(Person person)
        {
            _context.Persons.Update(person);
            _context.SaveChanges();
            return person;
        }

        public Person? GetById(long id)
        {
            return _context.Persons.Find(id);
        }

        public Person? GetByUserId(long userId)
        {
            return _context.Persons.FirstOrDefault(p => p.UserId == userId);
        }
        public void Delete(long personId)
        {
            var person = _context.Persons.Find(personId);
            if (person != null)
            {
                _context.Persons.Remove(person);
                _context.SaveChanges();
            }
        }
    }
}
