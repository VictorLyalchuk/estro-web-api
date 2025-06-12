using Core.Helpers;
using System;
using System.Net;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApi
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (CustomHttpException exception)
            {
                await CreateResponse(context, exception.StatusCode, exception.Message);
            }
            catch (KeyNotFoundException exception)
            {
                await CreateResponse(context, HttpStatusCode.NotFound, exception.Message);
            }
            catch (Exception exception)
            {
                await CreateResponse(context, HttpStatusCode.NotFound, exception.Message);
            }
        }
        private async Task CreateResponse(HttpContext context, HttpStatusCode statusCode = HttpStatusCode.InternalServerError, string message = "Unknown")
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var result = JsonSerializer.Serialize(new { message });
            await context.Response.WriteAsync(result);
        }
    }
}
