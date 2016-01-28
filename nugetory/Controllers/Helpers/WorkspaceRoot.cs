using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace nugetory.Controllers.Helpers
{
    public static class WorkspaceRoot
    {
        public static HttpResponseMessage GetWorkspace(HttpRequestMessage request)
        {
            Uri uri = request.RequestUri;

            string result = "<?xml version='1.0' encoding='utf-8' standalone='yes'?>";
            result += "<service xml:base=\"" + uri + "\" ";
            result += "xmlns:atom=\"http://www.w3.org/2005/Atom\" ";
            result += "xmlns:app=\"http://www.w3.org/2007/app\" ";
            result += "xmlns=\"http://www.w3.org/2007/app\">";
            result += "<workspace>";
            result += "<atom:title>Default</atom:title>";
            result += "<collection href=\"Packages\">";
            result += "<atom:title>Packages</atom:title>";
            result += "</collection>";
            result += "</workspace>";
            result += "</service>";

            HttpResponseMessage res = request.CreateResponse(HttpStatusCode.OK);
            res.Content = new StringContent(result, Encoding.UTF8, "text/xml");

            return res;
        }
    }
}
