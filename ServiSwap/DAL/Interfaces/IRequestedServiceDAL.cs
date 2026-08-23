using ServiSwap.Models;

namespace ServiSwap.DAL.Interfaces
{
    public interface IRequestedServiceDAL
    {
        public bool CreateRequestedService(RequestedService requestedService);

        public List<RequestedService> FindAll();

        public RequestedService FindById(int requestedServiceId);

        public bool Complete(RequestedService requestedService);
    }
}