using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using nugetory.Exceptions;

namespace nugetory.Endpoint
{
    public class ValidateAuthenticationAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (SkipAuthorization(actionContext))
                return;

            try
            {
                HttpRequestMessage request = actionContext.ControllerContext.Request;
                if (ValidateRequestHeader(request))
                    return;
            }
            catch (Exception)
            {
                throw new NotAuthorizedException();
            }
            throw new NotAuthorizedException();
        }

        private static bool SkipAuthorization(HttpActionContext actionContext)
        {
            if (actionContext == null)
                return false;

            return actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any()
                   ||
                   actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>()
                                .Any();
        }

        private bool ValidateRequestHeader(HttpRequestMessage request)
        {
            //string tokenValue = HttpRequestHeaderAuthToken.GetTokenFromHeader(request.Headers);
            //if (string.IsNullOrWhiteSpace(tokenValue))
            //    return false;

            //var token = Guid.Parse(tokenValue);
            //return Core.Aaa.Authentication.AuthManager.Validate(token.ToString());
            return true;
        }
    }
}
