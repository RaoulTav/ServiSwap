using ServiSwap.Models;

namespace ServiSwap.DAL.Interfaces
{
    public interface ICategoryDAL
    {
        public List<Category> FindAll();

        public Category? FindById(int categoryId);
    }
}