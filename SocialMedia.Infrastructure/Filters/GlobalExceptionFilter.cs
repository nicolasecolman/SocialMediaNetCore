using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SocialMedia.Core.Exceptions;
using System.Net;

namespace SocialMedia.Infrastructure.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext.Exception.GetType() == typeof(BusinessException))
            {
                var exception = (BusinessException)filterContext.Exception;
                var validation = new
                {
                    Status = 400,
                    Title = "Bad request",
                    Detail = exception.Message
                };

                var json = new
                {
                    errors = new[] { validation }
                };

                filterContext.Result = new BadRequestObjectResult(json);
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                filterContext.ExceptionHandled = true;
            }
        }
    }
}
