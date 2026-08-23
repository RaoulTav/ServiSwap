using ServiSwap.Models;

namespace ServiSwap.DAL.Interfaces
{
    public interface IPersonDAL
    {
        public int CreatePerson(Person person);

        public List<Person> FindAll();

        public Person FindById(int personId);

        public bool Exists(string username, string email);

        public Person FindByCredentials(string username, string password);
    }
}