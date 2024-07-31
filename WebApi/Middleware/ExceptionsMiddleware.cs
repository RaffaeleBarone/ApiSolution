using JsonPlaceholderWebApi.Exceptions;
using Microsoft.Extensions.Logging; 
using Newtonsoft.Json;
using System;
using System.Net;

namespace JsonPlaceholderWebApi.Middleware
{
    public class ExceptionsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionsMiddleware> _logger; 

        public ExceptionsMiddleware(RequestDelegate next, ILogger<ExceptionsMiddleware> logger)
        {
            _next = next;
            _logger = logger;   
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
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
                    code = HttpStatusCode.BadRequest; //400
                    break;
                case ForbiddenException:
                    code = HttpStatusCode.Forbidden; //403
                    break;
                case InternalServerErrorException:
                    code = HttpStatusCode.InternalServerError; //500
                    break;
                case NotFoundException:
                    code = HttpStatusCode.NotFound;   //404
                    break;
                case UnauthorizedException:
                    code = HttpStatusCode.Unauthorized;   //401
                    break;
                default:
                    code = HttpStatusCode.InternalServerError;
                    break;
            }

            result = JsonConvert.SerializeObject(new { error = ex.Message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
