using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using nugetory.Controllers.Helpers;
using nugetory.Exceptions;
using nugetory.Logging;

namespace nugetory.Controllers
{
    public class SearchController : ApiController
    {
        private static readonly ILogger Log = LogFactory.Instance.GetLogger(typeof (SearchController));

        [AllowAnonymous]
        public HttpResponseMessage Get()
        {
            Log.Submit(LogLevel.Debug, "GET request received");

            Dictionary<string, string> queryParams =
                Request.GetQueryNameValuePairs()
                       .ToDictionary(reqQueryParam => reqQueryParam.Key.ToLowerInvariant(),
                           reqQueryParam => reqQueryParam.Value);

            Func<string, string> getParam = s => queryParams.ContainsKey(s) ? queryParams[s] : null;

            string filter = getParam("$filter");
            string orderby = getParam("$orderby");
            string searchTerm = getParam("searchTerm");
            string targetFramework = getParam("targetFramework");
            string includePrereleaseStr = getParam("includePrerelease");
            string skipStr = getParam("$skip");
            string stopStr = getParam("$stop");
            bool desc = false;
            int skip;
            int stop;
            bool includePrerelase;

            if (orderby != null && orderby.Contains(" "))
            {
                string[] orderBySplit = orderby.Split(' ');
                if (orderBySplit.Length != 2)
                    throw new BadRequestException();
                orderby = orderBySplit[0];
                desc = orderBySplit[1] == "desc";
            }

            int.TryParse(skipStr, out skip);
            int.TryParse(stopStr, out stop);
            bool.TryParse(includePrereleaseStr, out includePrerelase);

            filter = filter != null ? filter.Trim(' ', '\'') : null;
            orderby = orderby != null ? orderby.Trim(' ', '\'') : null;
            searchTerm = searchTerm != null ? searchTerm.Trim(' ', '\'') : null;
            targetFramework = targetFramework != null ? targetFramework.Trim(' ', '\'') : null;

            HttpResponseMessage response = Search.Invoke(Request, filter, orderby, desc, searchTerm, targetFramework,
                includePrerelase, skip, stop);
            Log.Submit(LogLevel.Debug, "GET response ready");
            return response;
        }
    }
}
