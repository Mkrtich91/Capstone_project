using GameStore.BusinessLayer.Interfaces.DataProvider;
using GameStore.BusinessLayer.Interfaces.Exceptions;
using GameStore.BusinessLayer.Interfaces.RequestDto;
using GameStore.BusinessLayer.Interfaces.ResponseDto;
using GameStore.BusinessLayer.Interfaces.Services;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace GameStore.BusinessLayer.Services
{
    public class BoxPaymentService : IBoxPaymentService
    {
        private readonly HttpClient _httpClient;
        private readonly IOrderService _orderService;
        private readonly PaymentSettings _paymentSettings;
        private static readonly Guid StubCustomerId = Guid.Parse("7dce8347-4181-4316-9210-302361340975");
        private const int MaxRetryAttempts = 3;

        public BoxPaymentService(HttpClient httpClient, IOrderService orderService, IOptions<PaymentSettings> paymentSettings)
        {
            _httpClient = httpClient;
            _orderService = orderService;
            _paymentSettings = paymentSettings.Value;
        }

        public async Task<GetPaymentResponse> ProcessPaymentAsync(PaymentRequest paymentRequest)
        {
            if (paymentRequest.Method != "IBox terminal")
            {
                throw new ArgumentException("Invalid payment method.");
            }

            var cartDetails = await _orderService.GetCartDetailsAsync(StubCustomerId);

            if (cartDetails == null || !cartDetails.Items.Any())
            {
                throw new NotFoundException("No open order found.");
            }

            var totalAmount = cartDetails.Items.Sum(item =>
            {
                var discountAmount = item.Price * item.Quantity * (item.Discount / 100.0);
                return (item.Price * item.Quantity) - discountAmount;
            });

            int retryCount = 0;
            GetPaymentResponse paymentResponse;

            do
            {
                paymentResponse = await SendPaymentRequestAsync(cartDetails.OrderId, totalAmount);
                retryCount++;
            } while (paymentResponse == null && retryCount < MaxRetryAttempts);

            if (paymentResponse == null)
            {
                throw new PaymentFailedException("Payment processing failed after multiple attempts.");
            }

            var response = new GetPaymentResponse
            {
                UserId = StubCustomerId,
                OrderId = cartDetails.OrderId,
                PaymentDate = DateTime.UtcNow,
                Sum = totalAmount,
            };

            return response;
        }

        private async Task<GetPaymentResponse> SendPaymentRequestAsync(Guid orderId, double totalAmount)
        {
            var paymentRequest = new
            {
                UserId = StubCustomerId,
                OrderId = orderId,
                Sum = totalAmount,
            };

            var paymentUrl = $"{_paymentSettings.BaseUrl}/api/payments/ibox";
            var response = await _httpClient.PostAsJsonAsync(paymentUrl, paymentRequest);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Payment request failed with status code {response.StatusCode}.");
            }

            return await response.Content.ReadFromJsonAsync<GetPaymentResponse>();
        }
    }
}
