using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PrivilegeMobileService
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;
        public ApiExceptionFilter(ILoggerFactory loggerfactory)
        {
            _logger = loggerfactory.CreateLogger(nameof(ApiExceptionFilter));
        }
        public override void OnException(ExceptionContext context)
        {
            _logger.LogError($"error:{context.Exception.Message} on {context.Exception.ToString()}");
            if (context?.Exception != null)
            {
                HandleExceptionAsync(context);
                //context.ExceptionHandled = true;
            }
        }

        private static void HandleExceptionAsync(ExceptionContext context)
        {
            var exception = context.Exception;

            //if (exception is NotfoundException)
            //    SetExceptionResult(context, exception, HttpStatusCode.NotFound);
            //else if (exception is MyUnauthorizedException)
            //    SetExceptionResult(context, exception, HttpStatusCode.Unauthorized);
            //else if (exception is MyException)
            //    SetExceptionResult(context, exception, HttpStatusCode.BadRequest);
            //else
                SetExceptionResult(context, exception, HttpStatusCode.InternalServerError);
        }

        private static void SetExceptionResult(ExceptionContext context, Exception exception, HttpStatusCode code)
        {
            context.Result = new JsonResult(exception)
            {
                StatusCode = (int)code
            };
        }
    }
}
