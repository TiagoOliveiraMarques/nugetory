using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using nugetory.Controllers.Helpers;
using nugetory.Exceptions;

namespace nugetory.Controllers
{
    public class FindPackageController : ApiController
    {
        [AllowAnonymous]
        public HttpResponseMessage Get()
        {
            Dictionary<string, string> queryParams =
                Request.GetQueryNameValuePairs()
                       .ToDictionary(reqQueryParam => reqQueryParam.Key.ToLowerInvariant(),
                                     reqQueryParam => reqQueryParam.Value);

            Func<string, string> getParam = s => queryParams.ContainsKey(s) ? queryParams[s] : null;

            string filter = getParam("$filter");
            string orderby = getParam("$orderby");
            bool desc = false;
            string top = getParam("$top");
            string id = getParam("id");

            if (orderby.Contains(" "))
            {
                string[] orderBySplit = orderby.Split(' ');
                if (orderBySplit.Length != 2)
                    throw new BadRequestException();
                orderby = orderBySplit[0];
                desc = orderBySplit[1] == "desc";
            }

            filter = filter != null ? filter.Trim(' ', '\'') : null;
            orderby = orderby != null ? orderby.Trim(' ', '\'') : null;
            top = top != null ? top.Trim(' ', '\'') : null;
            id = id != null ? id.Trim(' ', '\'') : null;

            return FindPackage.Find(Request, filter, orderby, desc, top, id);
        }
    }
}
