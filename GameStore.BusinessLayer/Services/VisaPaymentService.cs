using GameStore.BusinessLayer.Interfaces.DataProvider;
using GameStore.BusinessLayer.Interfaces.DTO;
using GameStore.BusinessLayer.Interfaces.RequestDto;
using GameStore.BusinessLayer.Interfaces.Services;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
namespace GameStore.BusinessLayer.Services
{
    public class VisaPaymentService : IVisaPaymentService
    {
        private readonly HttpClient _httpClient;
        private readonly PaymentSettings _paymentSettings;

        public VisaPaymentService(HttpClient httpClient, IOptions<PaymentSettings> paymentSettings)
        {
            _httpClient = httpClient;
            _paymentSettings = paymentSettings.Value;
        }

        public async Task ProcessVisaPaymentAsync(VisaPaymentModelDto visaPaymentModel)
        {
            var visaPaymentRequest = new VisaPaymentRequest
            {
                CardHolderName = visaPaymentModel.Holder,
                CardNumber = visaPaymentModel.CardNumber,
                ExpirationMonth = visaPaymentModel.MonthExpire,
                ExpirationYear = visaPaymentModel.YearExpire,
                Cvv = visaPaymentModel.Cvv2,
            };

            var paymentUrl = $"{_paymentSettings.BaseUrl}/api/payments/visa";

            var response = await _httpClient.PostAsJsonAsync(paymentUrl, visaPaymentRequest);

            if (!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Visa payment processing failed. Status Code: {response.StatusCode}, Response: {responseContent}");
            }
        }
    }
}
