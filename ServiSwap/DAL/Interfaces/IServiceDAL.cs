using ServiSwap.Models;

namespace ServiSwap.DAL.Interfaces
{
    public interface IServiceDAL
    {
        public void CreateService(Service service);

        public List<Service> FindAll();

        public Service? FindById(int serviceId);

        public List<Service> SearchByCategoryAndOrDistance(double? latitude = null, double? longitude = null, int? categoryId = null, double? distance = null);
    }
}