using ServiSwap.Models;

namespace ServiSwap.DAL.Interfaces
{
    public interface IRequestedServiceDAL
    {
        public void CreateRequestedService(RequestedService requestedService);

        public RequestedService? FindById(int requestedServiceId);

        public void ReportHours(RequestedService requestedService);

        public void ConfirmHours(RequestedService requestedService);

        public void FindAllByRequester(Person requester);

        public void FindAllByProvider(Person provider);
    }
}