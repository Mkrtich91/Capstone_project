using GameStore.BusinessLayer.Interfaces.DTO;
using GameStore.BusinessLayer.Interfaces.RequestDto;
using GameStore.BusinessLayer.Interfaces.Services;
using GameStore.DataAccessLayer.Interfaces.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.BusinessLayer.Services
{
    public class PaymentProcessingService : IPaymentProcessingService
    {
        private readonly IPaymentBankService _paymentBankService;
        private readonly IBoxPaymentService _boxPaymentService;
        private readonly IVisaPaymentService _visaPaymentService;
        private readonly IOrderService _orderService;
        private static readonly Guid StubCustomerId = Guid.Parse("7dce8347-4181-4316-9210-302361340975");

        public PaymentProcessingService(
            IPaymentBankService paymentBankService,
            IBoxPaymentService boxPaymentService,
            IVisaPaymentService visaPaymentService,
            IOrderService orderService)
        {
            _paymentBankService = paymentBankService;
            _boxPaymentService = boxPaymentService;
            _visaPaymentService = visaPaymentService;
            _orderService = orderService;
        }

        public async Task<IActionResult> ProcessPaymentAsync(PaymentRequest paymentRequest)
        {
            var cartDetails = await _orderService.GetCartDetailsAsync(StubCustomerId);
            if (cartDetails == null || !cartDetails.Items.Any())
            {
                return new NotFoundObjectResult("No open order found.");
            }

            try
            {
                if (paymentRequest.Method == "IBox terminal")
                {
                    Console.WriteLine($"Processing IBox terminal payment for order {cartDetails.OrderId}");

                    var paymentResponse = await _boxPaymentService.ProcessPaymentAsync(paymentRequest);

                    Console.WriteLine($"IBox terminal payment response: {paymentResponse}");

                    await _orderService.UpdateOrderStatusAsync(cartDetails.OrderId, OrderStatus.Paid);
                    return new OkObjectResult(paymentResponse);
                }

                if (paymentRequest.Method == "Visa")
                {
                    if (paymentRequest.Model is VisaPaymentModelDto visaPaymentModel)
                    {
                        await _visaPaymentService.ProcessVisaPaymentAsync(visaPaymentModel);
                        await _orderService.UpdateOrderStatusAsync(cartDetails.OrderId, OrderStatus.Paid);
                        return new OkResult();
                    }
                    else
                    {
                        return new BadRequestObjectResult("Invalid Visa payment model.");
                    }
                }

                var pdfBytes = await _paymentBankService.ProcessPaymentAsync(paymentRequest);
                await _orderService.UpdateOrderStatusAsync(cartDetails.OrderId, OrderStatus.Paid);
                return new FileContentResult(pdfBytes, "application/pdf")
                {
                    FileDownloadName = "invoice.pdf"
                };
            }
            catch (Exception ex)
            {
                await _orderService.UpdateOrderStatusAsync(cartDetails.OrderId, OrderStatus.Cancelled);
                return new BadRequestObjectResult($"Payment processing failed: {ex.Message}");
            }
        }
    }
}
