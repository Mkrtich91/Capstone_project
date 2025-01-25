namespace GameStore.Middleware
{
    using System.Net;
    using GameStore.BusinessLayer.Interfaces.Exceptions;
    using Newtonsoft.Json;

    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            switch (exception)
            {
                case NotFoundException _:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                case ArgumentException _:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var errorResponse = new ErrorResponse
            {
                StatusCode = context.Response.StatusCode,
                Message = exception.Message,
            };

            await context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
        }
    }
}
