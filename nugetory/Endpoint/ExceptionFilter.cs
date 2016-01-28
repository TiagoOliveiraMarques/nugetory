using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http.Filters;
using nugetory.Exceptions;

namespace nugetory.Endpoint
{
    internal class ExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            Type exceptionType = context.Exception.GetType();
            if (exceptionType == typeof(NotAuthorizedException))
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }
            else if (exceptionType == typeof(PackageNotFoundException))
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Package version not found", Encoding.UTF8, new MediaTypeHeaderValue("text/plain").ToString()),
                };
                context.Response.Headers.Add("Status", "404 Package version not found");
            }
            else if (exceptionType == typeof(InternalServerErrorException))
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
            else if (exceptionType == typeof(BadRequestException))
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            else
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}
