using ServiSwap.Models;

namespace ServiSwap.DAL.Interfaces
{
    public interface IServiceDAL
    {
        public bool CreateService(Service service);

        public List<Service> FindAll();

        public Service FindById(int serviceId);

        public List<Service> SearchByCategoryAndOrDistance(int categoryId, double distance, double latitude, double longitude);
    }
}