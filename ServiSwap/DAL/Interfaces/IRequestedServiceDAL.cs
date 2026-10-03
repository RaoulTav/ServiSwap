using ServiSwap.Models;

namespace ServiSwap.DAL.Interfaces
{
    public interface IRequestedServiceDAL
    {
        public void CreateRequestedService(RequestedService requestedService);

        public RequestedService? FindById(int requestedServiceId);

        public bool Complete(RequestedService requestedService);
    }
}