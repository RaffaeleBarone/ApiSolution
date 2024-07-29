using JsonPlaceholderWebApi.Exceptions;
using Newtonsoft.Json;
using System;
using System.Net;

namespace JsonPlaceholderWebApi.Middleware
{
    public class ExceptionsMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
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

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            HttpStatusCode code;
            string result;

            switch(ex)
            {
                case BadRequestException:
                    code = HttpStatusCode.BadRequest;
                    break;
                case ForbiddenException:
                    code = HttpStatusCode.Forbidden; 
                    break;
                case InternalServerErrorException:
                    code = HttpStatusCode.InternalServerError;
                    break;
                case NotFoundException:
                    code = HttpStatusCode.NotFound;
                    break;
                case UnauthorizedException:
                    code = HttpStatusCode.Unauthorized;
                    break;
                default:
                    code = HttpStatusCode.BadRequest;
                    break;
            }

            result = JsonConvert.SerializeObject(new { error = ex.Message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
