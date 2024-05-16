using System.Net.Mime;
using System.Net;
using DocumentProcessor.Exceptions;
using System.Text.Json;

namespace DocumentProcessor.Middlewares
{
    public class ExceptionHandlingMiddleware(RequestDelegate next, IHostEnvironment environment)
    {
        private readonly RequestDelegate _next = next;
        private readonly IHostEnvironment _environment = environment;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ItemNotFoundException ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError);
            }
        }

        private Task HandleExceptionAsync<T>(HttpContext context, T ex, HttpStatusCode code) where T : Exception
        {
            context.Response.ContentType = MediaTypeNames.Application.Json;
            var errorId = Guid.NewGuid();
            context.Response.StatusCode = (int)code;

            var msg = _environment.IsDevelopment()
                ? $"{ex.Message}. Error reference id: {errorId}"
                : $"Error reference id: {errorId}";
            return context.Response.WriteAsync(JsonSerializer.Serialize(msg));
        }
    }
}
