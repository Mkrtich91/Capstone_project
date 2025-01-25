namespace GameStore.BusinessLayer.Services
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using GameStore.BusinessLayer.Interfaces.Exceptions;
    using GameStore.BusinessLayer.Interfaces.RequestDto;
    using GameStore.BusinessLayer.Interfaces.Services;
    using iText.Kernel.Pdf;
    using iText.Layout;
    using iText.Layout.Element;
    using Microsoft.Extensions.Configuration;

    public class PaymentBankService : IPaymentBankService
    {
        private readonly IConfiguration _configuration;
        private readonly IOrderService _orderService;
        private static readonly Guid StubCustomerId = Guid.Parse("7dce8347-4181-4316-9210-302361340975");

        public PaymentBankService(IConfiguration configuration, IOrderService orderService)
        {
            _configuration = configuration;
            _orderService = orderService;
        }

        public async Task<byte[]> GenerateInvoicePdfAsync(Guid customerId, Guid orderId, DateTime creationDate, DateTime validityDate, double sum)
        {
            using var ms = new MemoryStream();
            var writer = new PdfWriter(ms);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);

            document.Add(new Paragraph($"User ID: {customerId}"));
            document.Add(new Paragraph($"Order ID: {orderId}"));
            document.Add(new Paragraph($"Creation Date: {creationDate.ToShortDateString()}"));
            document.Add(new Paragraph($"Validity Date: {validityDate.ToShortDateString()}"));
            document.Add(new Paragraph($"Sum: {sum:C}"));

            document.Close();
            return ms.ToArray();
        }

        public async Task<byte[]> ProcessPaymentAsync(PaymentRequest paymentRequest)
        {
            if (paymentRequest.Method != "Bank")
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

            var invoiceDate = DateTime.UtcNow;
            var validityDate = invoiceDate.AddDays(int.Parse(_configuration["BankPaymentOptions:InvoiceValidityDays"]));
            var invoicePdf = await GenerateInvoicePdfAsync(StubCustomerId, cartDetails.OrderId, invoiceDate, validityDate, totalAmount);

            return invoicePdf;
        }
    }
}
