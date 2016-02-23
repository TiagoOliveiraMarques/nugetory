using System;
using System.Collections.Generic;
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
        public static string ApiKey { get; set; }

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

            HttpActionDescriptor actionDesc = actionContext.ActionDescriptor;
            HttpControllerDescriptor controllerDesc = actionContext.ControllerContext.ControllerDescriptor;

            return actionDesc.GetCustomAttributes<AllowAnonymousAttribute>().Any() ||
                   controllerDesc.GetCustomAttributes<AllowAnonymousAttribute>().Any();
        }

        private static bool ValidateRequestHeader(HttpRequestMessage request)
        {
            if (string.IsNullOrEmpty(ApiKey))
                return true;

            string apiKey;
            try
            {
                IEnumerable<string> nugetApiKeyValues = request.Headers.GetValues("X-Nuget-ApiKey");
                apiKey = nugetApiKeyValues.FirstOrDefault();
            }
            catch (Exception)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(apiKey))
                return false;

            return ApiKey == apiKey;
        }
    }
}
