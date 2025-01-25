namespace GameStore.BusinessLayer.Services
{
    using GameStore.BusinessLayer.Interfaces.DataProvider;
    using GameStore.BusinessLayer.Interfaces.DTO;
    using GameStore.BusinessLayer.Interfaces.Services;

    public class PaymentService : IPaymentService
    {
        public IEnumerable<PaymentMethodDto> GetPaymentMethods()
        {
            return PaymentMethods.Methods;
        }
    }
}
