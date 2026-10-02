using ServiSwap.Models;

namespace ServiSwap.DAL.Interfaces
{
    public interface IPersonDAL
    {
        public void CreatePerson(Person person);

        public Person? FindById(int personId);

        public bool Exists(string username, string email);

        public Person? FindByCredentials(string username, string password);
    }
}