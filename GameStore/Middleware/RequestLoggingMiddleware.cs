using System.Diagnostics;
using System.Text;

namespace GameStore.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            var originalBodyStream = context.Response.Body;

            try
            {
                var request = context.Request;
                var requestBody = await FormatRequest(request);

                using (var responseBodyStream = new MemoryStream())
                {
                    context.Response.Body = responseBodyStream;

                    await _next(context);

                    var response = context.Response;
                    var responseBody = await FormatResponse(responseBodyStream);

                    _logger.LogInformation($"User IP Address: {context.Connection.RemoteIpAddress}");
                    _logger.LogInformation($"Target URL: {request.Scheme}://{request.Host}{request.Path}");
                    _logger.LogInformation($"Request Method: {request.Method}");
                    _logger.LogInformation($"Request Content: {requestBody}");
                    _logger.LogInformation($"Response Status Code: {response.StatusCode}");
                    _logger.LogInformation($"Response Content: {responseBody}");

                    responseBodyStream.Seek(0, SeekOrigin.Begin);
                    await responseBodyStream.CopyToAsync(originalBodyStream);
                }
            }
            finally
            {
                stopwatch.Stop();
                _logger.LogInformation($"Elapsed Time: {stopwatch.ElapsedMilliseconds} ms");

                context.Response.Body = originalBodyStream;
            }
        }

        private async Task<string> FormatRequest(HttpRequest request)
        {
            request.EnableBuffering();
            var body = request.Body;
            byte[] buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            var requestBody = Encoding.UTF8.GetString(buffer);
            request.Body.Seek(0, SeekOrigin.Begin);
            return requestBody;
        }

        private async Task<string> FormatResponse(Stream responseBodyStream)
        {
            responseBodyStream.Seek(0, SeekOrigin.Begin);
            string responseBody = await new StreamReader(responseBodyStream).ReadToEndAsync();
            return responseBody;
        }
    }
}
